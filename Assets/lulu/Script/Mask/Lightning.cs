using UnityEngine;

public class Lightning : UseEffect
{
    [SerializeField]
    private float DmgLightning = 25f;
    private float ProjectileSpeed = 20f;
    private string OwnerUserId;

    void Start()
    {
        // Récupère l'ID du joueur propriétaire
        PlayerController2D playerController = GetComponentInParent<PlayerController2D>();
        if (playerController != null)
        {
            //ownerUserId = playerController.userId;
        }
    }
    
    
    public override void Use()
    {
        
    }
}
