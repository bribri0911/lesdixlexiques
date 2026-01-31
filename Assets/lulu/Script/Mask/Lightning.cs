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
            // 2. On instancie le projectile à la position actuelle
            GameObject projObj = Instantiate(gOLightnig, transform.position, Quaternion.identity);
            projObj.transform.SetParent(player.transform, false);
            projObj.transform.localPosition = Vector3.zero;

            // 3. On récupère le script Projectile pour le configurer
            Projectile projScript = projObj.GetComponent<Projectile>();
            
            // Sécurité : si lastDirection est zéro (joueur immobile au début), on tire à droite par défaut
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgLightning);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Lightning !");
        }
    }
}
