using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public string userId;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 targetVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    // Cette fonction est appelée par la Factory
    public void Move(Vector2 direction)
    {
        targetVelocity = direction * moveSpeed;
    }

    void FixedUpdate()
    {
        rb.linearVelocity = targetVelocity;
        
        // On reset la vélocité cible pour que le joueur s'arrête 
        // s'il ne reçoit plus d'inputs du WebSocket
        targetVelocity = Vector2.zero;
    }
}