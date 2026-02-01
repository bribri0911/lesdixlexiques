using UnityEngine;

public class Aura_Fire : UseEffect
{
    [SerializeField] private float DmgAura_Fire = 16f;
    [SerializeField] private GameObject gOAura_Fire;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null && player.GetComponentInChildren<AuraBase>() == null)
        {
            GameObject aura = Instantiate(gOAura_Fire, player.transform.position, Quaternion.identity);
            aura.transform.SetParent(player.transform);
            aura.GetComponent<AuraBase>()?.Setup(DmgAura_Fire, player.userId);
        }
    }
}