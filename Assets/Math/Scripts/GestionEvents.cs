using UnityEngine;
using System.Collections;

public class GestionEvents : MonoBehaviour
{
    public bool pvpActif = false;
    public float timeSetPVP = 30f;
    
    private bool canTogglePVP = false;
    private Coroutine pvpCoroutine;

    [SerializeField]
    private GameObject _FireBallEventPrefab;


    void OnEnable()
    {
        
        WebsocketManage.OnIceWorld += HandleActionCreateFireBall;
    }

    void OnDisable()
    {
        
        WebsocketManage.OnIceWorld -= HandleActionCreateFireBall;
    }

    void HandleActionCreateFireBall()
    {
        
    }


    void Start()
    {
        pvpCoroutine = StartCoroutine(AutoActivatePVP());
    }

    private IEnumerator AutoActivatePVP()
    {
        yield return new WaitForSeconds(timeSetPVP);
        
        pvpActif = true;
        canTogglePVP = true; 
        
        Debug.Log("⚔️ PVP activé ! Le contrôle manuel est désormais disponible.");
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