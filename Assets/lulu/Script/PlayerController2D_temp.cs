using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D_temp : MonoBehaviour
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
        getMask.AddMask(prefabTemps);
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