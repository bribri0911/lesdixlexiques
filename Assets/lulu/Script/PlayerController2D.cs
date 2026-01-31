using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Réglages")]
    public float moveSpeed = 5f;
    public string userId; // À remplir pour identifier quel joueur ce script contrôle

    private Rigidbody2D rb;
    private Vector2 currentMoveDir;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // On désactive la gravité si c'est une vue de dessus (Top-Down)
        rb.gravityScale = 0; 
    }

    void OnEnable()
    {
        // On s'abonne aux événements du WebSocket
        WebsocketManage.OnMoovePlayer += HandleMovement;
        WebsocketManage.OnUseMask += HandleUseMask;
        WebsocketManage.OnGetMask += HandleGetMask;
    }

    void OnDisable()
    {
        // TRÈS IMPORTANT : Se désabonner pour éviter les erreurs
        WebsocketManage.OnMoovePlayer -= HandleMovement;
        WebsocketManage.OnUseMask -= HandleUseMask;
        WebsocketManage.OnGetMask -= HandleGetMask;
    }

    // Cette fonction est appelée dès que le WebSocket reçoit un mouvement
    private void HandleMovement(string id, Vector2 direction)
    {
        // Si tu as plusieurs joueurs, on vérifie que l'ID correspond
        if (id == userId || string.IsNullOrEmpty(userId))
        {
            currentMoveDir = direction;
        }
    }

    private void HandleUseMask(string id)
    {
        if (id == userId || string.IsNullOrEmpty(userId))
        {
            Debug.Log($"🎭 Le joueur {id} utilise son masque !");
            // Ajoute ici ta logique visuelle (animation, effet, etc.)
        }
    }

    private void HandleGetMask(string id)
    {
         if (id == userId || string.IsNullOrEmpty(userId))
        {
            Debug.Log($"📦 Le joueur {id} ramasse un masque !");
        }
    }

    void FixedUpdate()
    {
        // Application du mouvement physique
        rb.linearVelocity = currentMoveDir * moveSpeed;

        // Optionnel : Arrêter le mouvement si on ne reçoit plus d'input 
        // (Sinon le perso glisse indéfiniment)
        currentMoveDir = Vector2.zero; 
    }
}