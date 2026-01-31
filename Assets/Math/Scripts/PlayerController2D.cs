using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public string userId;
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 targetVelocity;
    
    [SerializeField]
    private GetMask getMask;
    [SerializeField]
    private GameObject prefabTemps; 


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
        rb.linearVelocity = targetVelocity;
        targetVelocity = Vector2.zero;
    }
}