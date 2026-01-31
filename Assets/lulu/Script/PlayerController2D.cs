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

    public void Move(Vector2 direction)
    {
        targetVelocity = direction * moveSpeed;
    }

    public void UseMask()
    {
        Debug.Log("Use Mask");
    }

    public void ChangeMask(float direction)
    {
        Debug.Log("Change Mask");
    }

    public void GetMask()
    {
        Debug.Log("Get Mask");
    }

    void FixedUpdate()
    {
        rb.linearVelocity = targetVelocity;
        targetVelocity = Vector2.zero;
    }
}