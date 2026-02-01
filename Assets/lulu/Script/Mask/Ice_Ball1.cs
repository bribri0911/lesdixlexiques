using UnityEngine;

public class Ice_Ball : UseEffect
{
    [SerializeField]
    private float DmgIce_Ball = 25f;

    [SerializeField]
    private float ProjectileSpeed = 5f;
    

    [SerializeField]
    private GameObject gOIce_Ball;
    public override void Use()
    {
        // 1. On cherche le PlayerController2D dans les parents de cet objet
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            GameObject projObj = Instantiate(gOIce_Ball, transform.position, Quaternion.identity);

            projObj.transform.parent = player.transform;

            Projectile projScript = projObj.GetComponent<Projectile>();
            
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgIce_Ball, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Ice_Ball !");
        }
    }


}
