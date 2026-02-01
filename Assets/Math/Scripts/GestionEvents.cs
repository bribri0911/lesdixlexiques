using UnityEngine;
using System.Collections;

public class GestionEvents : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip startSound;

    public bool pvpActif = false;
    public float timeSetPVP = 30f;
    public bool StartGame = false;

    private bool canTogglePVP = false;
    private Coroutine pvpCoroutine;

    [SerializeField] private GameObject _FireBallEventPrefab;

    void OnEnable()
    {
        WebsocketManage.OnIceWorld += HandleActionCreateFireBall;
        WebsocketManage.OnStartGame += HandleActionStartGame;
        WebsocketManage.OnResetGame += HandleRestartGame;
    }

    void OnDisable()
    {
        WebsocketManage.OnIceWorld -= HandleActionCreateFireBall;
        WebsocketManage.OnStartGame -= HandleActionStartGame;
        WebsocketManage.OnResetGame -= HandleRestartGame;
    }

    void HandleActionStartGame()
    {
        if (!StartGame)
        {
            StartGame = true;
            if (audioSource != null && startSound != null)
            {
                audioSource.clip = startSound;
                audioSource.Play();
            }
            pvpCoroutine = StartCoroutine(AutoActivatePVP());
        }
    }

    void HandleRestartGame()
    {
        if (audioSource != null)
        {
            audioSource.Stop(); 
            audioSource.clip = null; 
        }

        if (pvpCoroutine != null)
        {
            StopCoroutine(pvpCoroutine);
            pvpCoroutine = null;
        }

        StartGame = false;
        pvpActif = false;
        canTogglePVP = false;
    }

    void HandleActionCreateFireBall() { }

    private IEnumerator AutoActivatePVP()
    {
        yield return new WaitForSeconds(timeSetPVP);
        pvpActif = true;
        canTogglePVP = true;
        Debug.Log("⚔️ PVP activé !");
    }

    public void SetPvpManual(bool state)
    {
        if (canTogglePVP)
        {
            pvpActif = state;
            Debug.Log(state ? "⚔️ PVP Activé manuellement." : "🛡️ PVP Désactivé manuellement.");
        }
        else
        {
            Debug.LogWarning("⏳ Impossible de modifier le PVP avant la fin du compte à rebours de 30s.");
        }
    }
}