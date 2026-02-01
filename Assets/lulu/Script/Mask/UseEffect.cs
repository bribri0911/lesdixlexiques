using UnityEngine;
using System.Collections;
using UnityEngine.UI; // Nécessaire pour l'UI

public abstract class UseEffect : MonoBehaviour
{
    [Header("Réglages Cooldown")]
    public float cooldown = 2f;
    public bool canUse = true;
    
    [Header("UI Feedback")]
    [SerializeField] public Image cooldownDisplayImage; // Une image radiale (Type: Filled)

    public float cooldownTimer = 0f;

    public abstract void Use();

    public void TryUse()
    {
        if (!canUse) return;

        canUse = false;
        cooldownTimer = cooldown; 
        Use();
        StartCoroutine(CooldownRoutine());
    }

    public IEnumerator CooldownRoutine()
    {
        while (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
            
            if (cooldownDisplayImage != null)
            {
                cooldownDisplayImage.fillAmount = cooldownTimer / cooldown;
            }
            
            yield return null; 
        }

        canUse = true;
        if (cooldownDisplayImage != null) cooldownDisplayImage.fillAmount = 0;
    }
}