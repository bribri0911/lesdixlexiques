using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class FactoryManager : MonoBehaviour
{
    public static FactoryManager Instance;

    [Header("Gestion Mort")]
    [SerializeField] private Vector3 deathZonePosition = new Vector3(-20, 0, 0);
    [SerializeField] private Vector3 spawnPosition = Vector3.zero;
    public List<string> deadPlayersIds = new List<string>();

    private Dictionary<string, PlayerController2D> deadPlayerDict = new Dictionary<string, PlayerController2D>();

    [Header("Configuration")]
    public GameObject playerPrefab;

    [Header("Suivi des Joueurs")]
    public Dictionary<string, PlayerController2D> playerDict = new Dictionary<string, PlayerController2D>();

    [SerializeField] private List<UserData> activePlayersDebug = new List<UserData>();

    public static event Action<int> OnActionInPlayerInGame;
    public static event Action OnWinGame;

    [SerializeField] private bool isGameStart = false;

    [System.Serializable]
    public class UserData
    {
        public string id;
        public Vector2 lastInput;
        public PlayerController2D controller;
    }

    void Awake() => Instance = this;

    void OnEnable()
    {
        WebsocketManage.OnIceWorld += HandleActionIceWorld;
        WebsocketManage.OnStartGame += HandleStartGame;
        WebsocketManage.OnMoovePlayer += HandleAction;
        WebsocketManage.OnUseMask += HandleUseMask;
        WebsocketManage.OnChangeMaskToLeft += HandheldChangeMaskLeft;
        WebsocketManage.OnChangeMaskToRight += HandheldChangeMaskRight;
        WebsocketManage.OnGetMask += HandheldGetMask;
        WebsocketManage.OnResetGame += HandleResetGame;
    }

    void OnDisable()
    {
        WebsocketManage.OnIceWorld -= HandleActionIceWorld;
        WebsocketManage.OnStartGame -= HandleStartGame;
        WebsocketManage.OnMoovePlayer -= HandleAction;
        WebsocketManage.OnUseMask -= HandleUseMask;
        WebsocketManage.OnChangeMaskToLeft -= HandheldChangeMaskLeft;
        WebsocketManage.OnChangeMaskToRight -= HandheldChangeMaskRight;
        WebsocketManage.OnGetMask -= HandheldGetMask;
        WebsocketManage.OnResetGame -= HandleResetGame;
    }

    void HandleStartGame() { if (!isGameStart) isGameStart = true; }

    private void HandleActionIceWorld() { StartCoroutine(IceWorldRoutine()); }

    private IEnumerator IceWorldRoutine()
    {
        foreach (var p in playerDict.Values) p.SetMovementState(2f, 0.5f);
        yield return new WaitForSeconds(10f);
        foreach (var p in playerDict.Values) p.ResetMovement();
    }

    private void HandleAction(string id, Vector2 moveDir)
    {
        if (string.IsNullOrEmpty(id)) return;
        if (!playerDict.ContainsKey(id))
        {
            // On vérifie si le joueur n'est pas simplement mort avant de tenter un spawn
            if (deadPlayerDict.ContainsKey(id)) return;

            PlayerController2D existingPlayer = FindPlayerByIdInScene(id);
            if (existingPlayer != null) playerDict.Add(id, existingPlayer);
            else { SpawnPlayer(id); return; }
        }

        if (playerDict.TryGetValue(id, out PlayerController2D controller))
        {
            controller.Move(moveDir);
            UpdateDebugInfo(id, moveDir);
        }
    }

    private void HandleUseMask(string id)

    {

        if (string.IsNullOrEmpty(id)) return;


        if (!playerDict.ContainsKey(id))
        {

            PlayerController2D existingPlayer = FindPlayerByIdInScene(id);


            if (existingPlayer != null)

            {

                playerDict.Add(id, existingPlayer);

            }

            else

            {

                SpawnPlayer(id);

                return;

            }

        }


        if (playerDict.TryGetValue(id, out PlayerController2D controller))

        {

            controller.UseMask();

        }

    }

    private void HandheldChangeMaskLeft(string id)

    {

        if (string.IsNullOrEmpty(id)) return;


        if (!playerDict.ContainsKey(id))

        {

            PlayerController2D existingPlayer = FindPlayerByIdInScene(id);


            if (existingPlayer != null)

            {

                playerDict.Add(id, existingPlayer);

            }

            else

            {

                SpawnPlayer(id);

                return;

            }

        }


        if (playerDict.TryGetValue(id, out PlayerController2D controller))

        {

            controller.ChangeMask(-1);

        }

    }

    private void HandheldChangeMaskRight(string id)

    {

        if (string.IsNullOrEmpty(id)) return;


        if (!playerDict.ContainsKey(id))

        {

            PlayerController2D existingPlayer = FindPlayerByIdInScene(id);


            if (existingPlayer != null)

            {

                playerDict.Add(id, existingPlayer);

            }

            else

            {

                SpawnPlayer(id);

                return;

            }

        }


        if (playerDict.TryGetValue(id, out PlayerController2D controller))

        {

            controller.ChangeMask(1);

        }

    }

    private void HandheldGetMask(string id)
    {
        if (string.IsNullOrEmpty(id)) return;

        if (!playerDict.ContainsKey(id))
        {
            PlayerController2D existingPlayer = FindPlayerByIdInScene(id);

            if (existingPlayer != null)
            {
                playerDict.Add(id, existingPlayer);
            }
            else
            {
                SpawnPlayer(id);
                return;
            }
        }

        if (playerDict.TryGetValue(id, out PlayerController2D controller))
        {
            controller.GetMaskFunction();
        }
    }

    private void SpawnPlayer(string id)
    {
        if (isGameStart) return;

        GameObject go = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        go.name = "Player_" + id.Substring(0, 6);
        PlayerController2D ctrl = go.GetComponent<PlayerController2D>();
        ctrl.userId = id;

        playerDict.Add(id, ctrl);
        activePlayersDebug.Add(new UserData { id = id, controller = ctrl });
        OnActionInPlayerInGame?.Invoke(playerDict.Count);
    }
    public void ReviveUser(string userId)
    {
        if (deadPlayerDict.ContainsKey(userId))
        {
            PlayerController2D player = deadPlayerDict[userId];

            player.transform.position = spawnPosition;

            // CORRECTION ICI : On active le GameObject directement
            player.gameObject.SetActive(true);
            player.enabled = true;

            playerDict.Add(userId, player);
            deadPlayerDict.Remove(userId);

            deadPlayersIds.Remove(userId);
            activePlayersDebug.Add(new UserData { id = userId, controller = player });

            OnActionInPlayerInGame?.Invoke(playerDict.Count);
        }
    }

    public void DeathUser(string userId)
    {
        if (playerDict.ContainsKey(userId))
        {
            PlayerController2D player = playerDict[userId];

            player.transform.position = deathZonePosition;

            // On désactive le visuel et le script
            player.enabled = false;
            player.gameObject.SetActive(false);

            deadPlayerDict.Add(userId, player);
            playerDict.Remove(userId);
            // ... reste du code identique
        }
    }

    void HandleResetGame()
    {
        isGameStart = false;
        ReviveAllPlayers();
        ResetMaskPlayer();
        ResetPv();
        OnActionInPlayerInGame?.Invoke(playerDict.Count);
    }

    public void ReviveAllPlayers()
    {
        List<string> toRevive = new List<string>(deadPlayerDict.Keys);
        foreach (string id in toRevive)
        {
            ReviveUser(id);
        }
    }

    public void ResetMaskPlayer()
    {
        foreach (PlayerController2D controller in playerDict.Values)
        {
            if (controller != null)
            {
                controller.RemoveAllMask();
            }
        }
        foreach (PlayerController2D deadController in deadPlayerDict.Values)
        {
            if (deadController != null)
            {
                deadController.RemoveAllMask();
            }
        }
    }

    public void ResetPv()
    {
        foreach (var controller in playerDict.Values)
        {
            if (controller != null)
            {
                Player_Point_De_Vie pvScript = controller.GetComponent<Player_Point_De_Vie>();
                if (pvScript != null)
                {
                    pvScript.ResetPv();
                }
            }
        }
        foreach (var deadController in deadPlayerDict.Values)
        {
            if (deadController != null)
            {
                Player_Point_De_Vie pvScript = deadController.GetComponent<Player_Point_De_Vie>();
                if (pvScript != null)
                {
                    pvScript.ResetPv();
                }
            }
        }

    }

    private PlayerController2D FindPlayerByIdInScene(string id)
    {
        PlayerController2D[] allPlayers = FindObjectsByType<PlayerController2D>(FindObjectsSortMode.None);
        foreach (var p in allPlayers)
        {
            if (p.userId == id) return p;
        }
        return null;
    }

    private void UpdateDebugInfo(string id, Vector2 input)
    {
        UserData data = activePlayersDebug.Find(x => x.id == id);
        if (data != null) data.lastInput = input;
    }

    public void RemovePlayer(string id)
    {
        if (playerDict.ContainsKey(id))
        {
            Destroy(playerDict[id].gameObject);
            playerDict.Remove(id);
            activePlayersDebug.RemoveAll(x => x.id == id);
        }
    }
}