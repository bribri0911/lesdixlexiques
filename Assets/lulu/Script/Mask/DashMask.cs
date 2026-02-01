using UnityEngine;
using System.Collections;

public class DashMask : UseEffect
{
    [SerializeField] private float dashSpeed = 45f; 
    [SerializeField] private float dashDuration = 0.3f;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null) StartCoroutine(DashRoutine(player));
    }

    private System.Collections.IEnumerator DashRoutine(PlayerController2D player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        float oldAccel = player.acceleration;
        player.SetMovementState(0f, 0f); 
        Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.down : player.lastDirection.normalized;
        rb.linearVelocity = dir * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = Vector2.zero;
        player.SetMovementState(oldAccel, 2f); // Remplace 2f par ta décélération par défaut
    }
}