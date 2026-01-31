using UnityEngine;

public class Mask_Object
{
    public int Id {get;set;} //identifié le mask
    public bool isPick { get; set; } = false; // Mask équipé
    public bool Can_Use = true; // Peux utiliser le mask
    public float Cooldown {get;} = 5f; // Durée du cooldown en secondes
    public float NextUseTime  = 0f; // Temps auquel le masque sera disponible

    public void SetUpUseAfterCooldown()
    {
        Can_Use = false;

    }




}

