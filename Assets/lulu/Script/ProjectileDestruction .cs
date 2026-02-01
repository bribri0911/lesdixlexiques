using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] 
public class ProjectileDestruction : Projectile
{
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
                Destroy(gameObject);
            }

            
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

    }
}
