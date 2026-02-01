using UnityEngine;
using System.Collections;

public class DashMask : UseEffect
{

    [SerializeField] private float dashSpeed = 20f; 
    [SerializeField] private float dashDuration = 0.2f;

    private bool isDashing = false;

    private float defaultAccel = 10f;
    private float defaultDecel = 2f;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null && !isDashing)
        {
            defaultAccel = player.acceleration;
            defaultDecel = player.deceleration;

            StartCoroutine(Dash(player));
        }
    }

    private IEnumerator Dash(PlayerController2D player)
    {
        isDashing = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();

        player.SetMovementState(0f, 0f); 

        Vector2 direction = player.lastDirection;
        if (direction == Vector2.zero) direction = Vector2.down;
        direction.Normalize();

        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;

        player.SetMovementState(defaultAccel, defaultDecel);

        isDashing = false;
    }
}
