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
            GameObject projObj = Instantiate(gOFire_Ball, transform.position, Quaternion.identity);

            projObj.transform.parent = player.transform;

            Projectile projScript = projObj.GetComponent<Projectile>();
            
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
