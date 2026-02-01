using UnityEngine;

public class Lightning : UseEffect
{
    [Header("Réglages Lightning")]
    [SerializeField] private float DmgLightning = 25f;
    [SerializeField] private float ProjectileSpeed = 200f;
    [SerializeField] private GameObject gOLightnig;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            GameObject projObj = Instantiate(gOLightnig, transform.position, Quaternion.identity);

            Projectile projScript = projObj.GetComponent<Projectile>();
            
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir, ProjectileSpeed, DmgLightning, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable pour le masque Lightning !");
        }
    }
}