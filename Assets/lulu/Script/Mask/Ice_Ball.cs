using UnityEngine;

public class Ice_Ball : UseEffect
{
    [SerializeField] private float DmgIce_Ball = 25f;
    [SerializeField] private float ProjectileSpeed = 5f;
    [SerializeField] private GameObject gOIce_Ball;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null)
        {
            GameObject projObj = Instantiate(gOIce_Ball, transform.position, Quaternion.identity);
            Projectile projScript = projObj.GetComponent<Projectile>();
            
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;
            if (projScript != null) projScript.Setup(dir, ProjectileSpeed, DmgIce_Ball, player.userId);
        }
    }
}
