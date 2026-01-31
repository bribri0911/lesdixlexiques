using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Force l'ajout d'un Rigidbody2D sur le préfab
public class Projectile : MonoBehaviour
{
    public float lifetime = 10f;
    private Rigidbody2D rb;
    public float damage; 
    public string userId;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        Destroy(gameObject, lifetime); 
    }

    public void Setup(Vector2 direction, float speed, float dmg, string idUser)
    {

        damage = dmg;
        userId = idUser;

        rb.linearVelocity = direction.normalized * speed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
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
