using UnityEngine;
using UnityEngine.UI;

public abstract class UseEffect : MonoBehaviour
{
    [Header("Réglages Cooldown")]
    public float cooldown = 2f;
    
    [Header("UI Feedback")]
    [SerializeField] public Image cooldownDisplayImage;

    // On stocke le moment précis où le cooldown sera fini
    private float nextReadyTime = 0f;

    public bool canUse => Time.time >= nextReadyTime;

    public abstract void Use();

    public void TryUse()
    {
        if (!canUse) return;

        // On définit le futur moment de disponibilité
        nextReadyTime = Time.time + cooldown;
        Use();
    }

    protected virtual void Update()
    {
        if (!canUse)
        {
            float remaining = nextReadyTime - Time.time;
            if (cooldownDisplayImage != null)
            {
                cooldownDisplayImage.fillAmount = remaining / cooldown;
            }
        }
        else
        {
            if (cooldownDisplayImage != null && cooldownDisplayImage.fillAmount != 0)
            {
                cooldownDisplayImage.fillAmount = 0;
            }
        }
    }

    // Sécurité : Quand on réactive le masque, on vérifie si le cooldown a expiré pendant l'absence
    protected virtual void OnEnable()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (cooldownDisplayImage != null)
        {
            float remaining = Mathf.Max(0, nextReadyTime - Time.time);
            cooldownDisplayImage.fillAmount = remaining / cooldown;
        }
    }
}