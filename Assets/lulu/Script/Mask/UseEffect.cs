using UnityEngine;
using System.Collections;

public abstract class UseEffect : MonoBehaviour
{
    public bool canUse = true;
    public abstract void Use();

    public float cooldown = 2f; // durée du cooldown en secondes
    private float nextTimeAvailable = 0f;

    public void TryUse()
    {
        if (!canUse) return;

        canUse = false;
        Use();
        StartCoroutine(CooldownRoutine());
    }
    private IEnumerator CooldownRoutine()
    {
        yield return new WaitForSeconds(cooldown);
        canUse = true;
    }
}
