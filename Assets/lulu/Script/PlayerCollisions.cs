using UnityEngine;
public class PlayerCollisions : MonoBehaviour
{
    [SerializeField] 
    Player_Point_De_Vie P;

    [SerializeField] 
    Lightning D;
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Lightning"))
        {
            P.GetDomage(100f);
        }
    }
}