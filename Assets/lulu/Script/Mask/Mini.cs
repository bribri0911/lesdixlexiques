using UnityEngine;

public class Mini : MonoBehaviour
{
    private Transform playerRoot;
    private Vector3 originalScale;

    void OnEnable()
    {
        playerRoot = transform.root;

        if (playerRoot != null && playerRoot != transform)
        {
            originalScale = playerRoot.localScale;

            playerRoot.localScale = new Vector2(0.5f, 0.5f);
        }
    }

    void OnDisable()
    {
        if (playerRoot != null)
        {
            playerRoot.localScale = originalScale;
        }
    }
}
