using UnityEngine;

public class Lightning : UseEffect
{
    [SerializeField]
    public float DmgLightning = 25f;
    [SerializeField]
    private float ProjectileSpeed = 5f;

    [SerializeField]
    private GameObject gOLightnig; // Assurez-vous que ce préfab a le script Projectile et un Rigidbody2D

    public override void Use()
    {
        // 1. On cherche le PlayerController2D dans les parents de cet objet
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            GameObject projObj = Instantiate(gOLightnig, transform.position, Quaternion.identity);

            projObj.transform.parent = player.transform;

            Projectile projScript = projObj.GetComponent<Projectile>();
            
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgLightning, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Lightning !");
        }
    }
}
