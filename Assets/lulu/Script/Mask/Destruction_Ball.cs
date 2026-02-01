using UnityEngine;

public class Destruction_Ball : UseEffect
{
    [SerializeField]
    private float DmgDestruction_Ball = 50f;

    [SerializeField]
    private float ProjectileSpeed = 15f;
    

    [SerializeField]
    private GameObject gODestruction_Ball;
    public override void Use()
    {
        // 1. On cherche le PlayerController2D dans les parents de cet objet
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            GameObject projObj = Instantiate(gODestruction_Ball, transform.position, Quaternion.identity);

            projObj.transform.parent = player.transform;

            Projectile projScript = projObj.GetComponent<Projectile>();
            
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgDestruction_Ball, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Destruction_Ball !");
        }
    }


}
