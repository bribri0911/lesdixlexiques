using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FactoryManager : MonoBehaviour
{
    public static FactoryManager Instance;

    [Header("Configuration")]
    public GameObject playerPrefab;

    [Header("Suivi des Joueurs")]
    public Dictionary<string, PlayerController2D> playerDict = new Dictionary<string, PlayerController2D>();

    [SerializeField] private List<UserData> activePlayersDebug = new List<UserData>();

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

        WebsocketManage.OnMoovePlayer += HandleAction;
        WebsocketManage.OnUseMask += HandleUseMask;
        WebsocketManage.OnChangeMaskToLeft += HandheldChangeMaskLeft;
        WebsocketManage.OnChangeMaskToRight += HandheldChangeMaskRight;
        WebsocketManage.OnGetMask += HandheldGetMask;
    }

    private void HandleActionIceWorld()
    {
        StartCoroutine(IceWorldRoutine());
    }

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

    private PlayerController2D FindPlayerByIdInScene(string id)
    {
        PlayerController2D[] allPlayers = FindObjectsByType<PlayerController2D>(FindObjectsSortMode.None);
        foreach (var p in allPlayers)
        {
            if (p.userId == id) return p;
        }
        return null;
    }

    private void SpawnPlayer(string id)
    {
        GameObject go = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        go.name = "Player_" + id.Substring(0, 6);

        PlayerController2D ctrl = go.GetComponent<PlayerController2D>();
        ctrl.userId = id;

        playerDict.Add(id, ctrl);

        activePlayersDebug.Add(new UserData { id = id, controller = ctrl });
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

    public void DeathUser(string userId)
    {
        if (playerDict.ContainsKey(userId))
        {
            playerDict.Remove(userId);
            activePlayersDebug.RemoveAll(x => x.id == userId);
        }
    }



}