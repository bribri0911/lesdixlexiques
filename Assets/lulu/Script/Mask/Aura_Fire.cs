using UnityEngine;

public class Aura_Fire : UseEffect
{
    [SerializeField]
    private float DmgAura_Fire = 16f;
    

    [SerializeField]
    private GameObject gOAura_Fire;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            // SÉCURITÉ : Vérifier si le joueur a déjà une aura pour éviter d'en empiler 50
            AuraBase existingAura = player.GetComponentInChildren<AuraBase>();
            if (existingAura != null)
            {
                // Si une aura existe déjà, on ne fait rien (ou on pourrait la détruire pour faire un "Toggle")
                return; 
            }

            // Création de l'aura
            GameObject auraObj = Instantiate(gOAura_Fire, transform.position, Quaternion.identity);
            
            // On colle l'aura au joueur (enfant du Transform du joueur)
            auraObj.transform.SetParent(player.transform, false);
            auraObj.transform.localPosition = Vector3.zero;

            // Configuration des dégâts
            AuraBase auraScript = auraObj.GetComponent<AuraBase>();
            if (auraScript != null)
            {
                auraScript.Setup(DmgAura_Fire, player.userId);
            }
        }
    }
}
