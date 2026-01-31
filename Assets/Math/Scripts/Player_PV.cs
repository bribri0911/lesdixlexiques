using UnityEngine;

public class Player_Point_De_Vie : MonoBehaviour
{
    [SerializeField] 
    private GameObject gOGestionEvents;
    private GestionEvents gestionEvents;
    public float Player_pv=100f;
    public readonly float Player_pv_max=100f;





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
        if (Player_pv < 0)
            gameObject.SetActive(false);

    }
  



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gOGestionEvents = GameObject.FindWithTag("EventsManager");
        gestionEvents = gOGestionEvents.GetComponent<GestionEvents>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
