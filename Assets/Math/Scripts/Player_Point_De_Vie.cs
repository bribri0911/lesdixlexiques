using UnityEngine;
public class Player_Point_De_Vie : MonoBehaviour
{
    [SerializeField] 
    private GameObject gOGestionEvents;
    private GestionEvents gestionEvents;
    public float Player_pv=100f;
    public readonly float Player_pv_max=100f;

    [SerializeField]
    private PlayerController2D playerController2D;


    


    [SerializeField]
    public void GetDomage(float DmgTake) 
    {

        if (gestionEvents.pvpActif)
        {
            Player_pv -= DmgTake;
            if (Player_pv>Player_pv_max)
            
                Player_pv = Player_pv_max;
            
            CheckIsDead();
        }


    }

    private void CheckIsDead()
    {
        if (Player_pv <= 0)
        {
            gameObject.SetActive(false);
            
            FactoryManager factoryManager = GameObject.FindWithTag("Factory")?.GetComponent<FactoryManager>();
            if(factoryManager != null)
            {
                factoryManager.DeathUser(playerController2D.userId);
            }
        }

    }

    public void ResetPv()
    {
        Player_pv = Player_pv_max;
    }
    
    
    void Start()
    {
        playerController2D = gameObject.GetComponent<PlayerController2D>();
        gOGestionEvents = GameObject.FindWithTag("EventsManager");
        gestionEvents = gOGestionEvents.GetComponent<GestionEvents>();
        Player_pv = Player_pv_max;
    }

    void Update()
    {
        
    }
}
