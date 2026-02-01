using UnityEngine;

public class Aura_Fire : UseEffect
{
    [SerializeField] private float DmgAura_Fire = 16f;
    [SerializeField] private GameObject gOAura_Fire;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null)
        {
            // Éviter de cumuler les auras
            if (player.GetComponentInChildren<AuraBase>() != null) return; 

            GameObject auraObj = Instantiate(gOAura_Fire, player.transform.position, Quaternion.identity);
            auraObj.transform.SetParent(player.transform);
            auraObj.transform.localPosition = Vector3.zero;

            AuraBase auraScript = auraObj.GetComponent<AuraBase>();
            if (auraScript != null) auraScript.Setup(DmgAura_Fire, player.userId);
        }
    }
}