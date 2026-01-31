using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public string userId;
    public float moveSpeed = 5f;

    [Header("Réglages Glisse")]
    public float acceleration = 10f;  // Sensibilité de la prise de vitesse
    public float deceleration = 2f;

    public Vector2 lastDirection;

    private Rigidbody2D rb;
    private Vector2 targetVelocity;

    [SerializeField]
    private GetMask getMask;
    [SerializeField]
    private GameObject prefabTemps;

    public void SetMovementState(float accel, float decel)
    {
        acceleration = accel;
        deceleration = decel;
    }

    public void ResetMovement()
    {
        acceleration = 10f;
        deceleration = 5f;
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    public void Move(Vector2 direction)
    {
        lastDirection = direction;
        targetVelocity = direction;
    }


    public void UseMask()
    {
        getMask.UseMaskActive();
    }

    public void ChangeMask(int direction)
    {
        getMask.ChangeMask(direction);
    }

    public void GetMaskFunction()
    {
        getMask.AddMask(prefabTemps);
    }

    void FixedUpdate()
    {
        if (targetVelocity != Vector2.zero)
        {
            Vector2 targetVel = targetVelocity * moveSpeed;
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVel, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, deceleration * Time.fixedDeltaTime);
        }

        targetVelocity = Vector2.zero;
    }
}