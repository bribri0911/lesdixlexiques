using UnityEngine;

public class Player_Point_De_Vie : MonoBehaviour
{
    [SerializeField] 
    private GameObject gOGestionEvents;
    private GestionEvents gestionEvents;
    public float Player_pv=100f;
    public float Time_Set_PVP = 30f;



    public void GetDomage(float DmgTake) 
    {

        if (gestionEvents.pvpActif)
        {
            Player_pv -= DmgTake;

            CheckIsDead();
        }


    }

    private void CheckIsDead()
    {
        //if (Player_pv < 0)
       //     this.SetActive(false);

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
