using UnityEngine;

public class AuraBase : MonoBehaviour
{
    public float damage;
    public string userId;

    // Pas de Start(), donc pas de destruction automatique !

    public void Setup(float dmg, string idUser)
    {
        damage = dmg;
        userId = idUser;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if(collision.tag == "Player")
        {
            GameObject temps = collision.gameObject;

            PlayerController2D playerController2D = temps.GetComponentInParent<PlayerController2D>();

            if(playerController2D.userId != userId)
            {
                Player_Point_De_Vie player_Point_De_Vie = temps.GetComponentInParent<Player_Point_De_Vie>();
                player_Point_De_Vie.GetDomage(damage);
            }
        }
    }
}
