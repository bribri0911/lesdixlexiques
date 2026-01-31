using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Force l'ajout d'un Rigidbody2D sur le préfab
public class AuraBase  : MonoBehaviour
{
    private float damagePerSecond;
    private float tickTimer = 0f;
    private float tickInterval = 1.5f; // Dégâts toutes les 1.5 secondes

    public void Setup(float duration, float dmg)
    {
        this.damagePerSecond = dmg;
        Destroy(gameObject, duration);
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