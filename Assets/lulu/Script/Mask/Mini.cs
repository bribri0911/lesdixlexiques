using UnityEngine;
using System.Collections;

public class Mini : UseEffect
{
    [Header("Réglages Mini")]
    [SerializeField] private float effectDuration = 5f; 
    [SerializeField] private Vector3 miniScale = new Vector3(0.5f, 0.5f, 1f);
    
    private Transform playerRoot;
    private Vector3 originalScale = Vector3.one; 
    private bool isEffectActive = false;

    private void Start()
    {
        playerRoot = transform.root;
    }

    public override void Use()
    {
        if (isEffectActive) return;
        StartCoroutine(MiniRoutine());
    }

    private IEnumerator MiniRoutine()
    {
        ApplyEffect();
        yield return new WaitForSeconds(effectDuration);
        ResetEffect();
    }

    private void ApplyEffect()
    {
        if (playerRoot != null && !isEffectActive)
        {
            isEffectActive = true;
            originalScale = playerRoot.localScale;
            playerRoot.localScale = miniScale;
            Debug.Log("👶 Effet Mini activé");
        }
    }

    private void ResetEffect()
    {
        if (playerRoot != null && isEffectActive)
        {
            playerRoot.localScale = originalScale;
            isEffectActive = false;
            Debug.Log("🏃 Taille normale rétablie");
        }
    }

    private void OnDisable()
    {
        if (isEffectActive)
        {
            StopAllCoroutines();
            ResetEffect();
        }
    }

    private void OnDestroy()
    {
        if (isEffectActive)
        {
            ResetEffect();
        }
    }
}