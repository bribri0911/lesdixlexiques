using UnityEngine;

public class Mask_Object
{
    public int Id {get;set;} //identifié le mask
    private bool isPick { get; set; } = false; // Mask équipé
    private bool Can_Use = true; // Peux utiliser le mask
    private float Cooldown {get;} = 5f; // Durée du cooldown en secondes
    private float NextUseTime  = 0f; // Temps auquel le masque sera disponible
    private void SetUpUseAfterCooldown()
    {
        Can_Use = false;
        Cooldown_Mask = 
        Debug.Log ($"Cooldown mask : {Cooldown_Mask}");
    }




}

