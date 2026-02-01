using UnityEngine;

public class Mini : UseEffect
{
    [Header("Réglages Passif Mini")]
    [SerializeField] private Vector3 miniScale = new Vector3(0.5f, 0.5f, 1f);
    [SerializeField] private Vector3 normalScale = Vector3.one;
    
    [SerializeField] private Transform playerTarget;

    private void Awake()
    {
        // On cherche la racine du joueur (Tag "Player") une seule fois
        playerTarget = transform.parent;
        while (playerTarget != null && !playerTarget.CompareTag("Player"))
        {
            playerTarget = playerTarget.parent;
        }
    }

    // On laisse Use vide car ce masque n'a pas de pouvoir "actif"
    public override void Use() 
    {
        Debug.Log("Ce masque est passif, il n'y a rien à activer !");
    }

    // Dès que le masque apparaît sur le joueur (Equipé)
    protected override void OnEnable()
    {
        base.OnEnable();
        if (playerTarget != null)
        {
            playerTarget.localScale = miniScale;
        }
    }

    // Dès que le masque disparaît ou qu'on change (Déséquipé)
    private void OnDisable()
    {
        if (playerTarget != null)
        {
            playerTarget.localScale = normalScale;
        }
    }

    // Sécurité si on jette le masque par terre
    private void OnDestroy()
    {
        if (playerTarget != null)
        {
            playerTarget.localScale = normalScale;
        }
    }
}