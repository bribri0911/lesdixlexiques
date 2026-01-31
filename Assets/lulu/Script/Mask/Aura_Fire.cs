using UnityEngine;

public class Aura_Fire : UseEffect
{
    [SerializeField]
    private float DmgAura_Fire = 16f;

    [SerializeField]
    private GameObject gOAura_Fire;
    public override void Use()
    {
        // 1. On cherche le PlayerController2D dans les parents de cet objet
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            // 2. On instancie le projectile à la position actuelle
            GameObject projObj = Instantiate(gOAura_Fire, transform.position, Quaternion.identity);
            projObj.transform.SetParent(player.transform, false);
            projObj.transform.localPosition = Vector3.zero;
            
            // 3. On récupère le script Projectile pour le configurer
            Projectile projScript = projObj.GetComponent<Projectile>();
              
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Fire_Ball !");
        }
    }
}
