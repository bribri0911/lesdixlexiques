using UnityEngine;

public class Fire_Ball : UseEffect
{
    [SerializeField]
    private float DmgFire_Ball = 10f;

    [SerializeField]
    private float ProjectileSpeed = 30f;

    [SerializeField]
    private GameObject gOFire_Ball;
    public override void Use()
    {
        // 1. On cherche le PlayerController2D dans les parents de cet objet
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            // 2. On instancie le projectile à la position actuelle
            GameObject projObj = Instantiate(gOFire_Ball, transform.position, Quaternion.identity);
            projObj.transform.parent = this.GetComponent<GameObject>().transform;

            // 3. On récupère le script Projectile pour le configurer
            Projectile projScript = projObj.GetComponent<Projectile>();
            

            // Sécurité : si lastDirection est zéro (joueur immobile au début), on tire à droite par défaut
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgFire_Ball, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Fire_Ball !");
        }
    }
}
