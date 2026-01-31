using UnityEngine;

public class GestionEvents : MonoBehaviour
{
    public bool pvpActif = false;
    public float Time_Set_PVP = 30f,Time_Start=0;

    public  void PVP()
    {
        if (Time_Start >= Time_Set_PVP)
            pvpActif = true;
    }
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Time_Start += Time.deltaTime;
        PVP();
    }
}

