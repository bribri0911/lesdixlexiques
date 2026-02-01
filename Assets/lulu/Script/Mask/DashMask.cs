using UnityEngine;
using System.Collections;

public class DashMask : UseEffect
{
    [SerializeField] private float dashSpeed = 45f; 
    [SerializeField] private float dashDuration = 0.3f;
    private bool isDashing = false;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null && !isDashing)
        {
            StartCoroutine(Dash(player));
        }
    }

    private IEnumerator Dash(PlayerController2D player)
    {
        isDashing = true;
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        
        float oldAccel = player.acceleration;
        float oldDecel = player.deceleration;

        player.SetMovementState(0f, 0f); 

        Vector2 direction = player.lastDirection == Vector2.zero ? Vector2.down : player.lastDirection.normalized;
        rb.linearVelocity = direction * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        rb.linearVelocity = Vector2.zero;
        player.SetMovementState(oldAccel, oldDecel);
        isDashing = false;
    }
}