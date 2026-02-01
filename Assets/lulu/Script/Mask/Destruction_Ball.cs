using UnityEngine;

public class Destruction_Ball : UseEffect
{
    [SerializeField] private float DmgDestruction_Ball = 50f;
    [SerializeField] private float ProjectileSpeed = 15f;
    [SerializeField] private GameObject gODestruction_Ball;
    [SerializeField] private float SelfStunTime = 1.5f;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null)
        {
            player.ModifMovement(SelfStunTime);
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;
            GameObject projObj = Instantiate(gODestruction_Ball, transform.position, Quaternion.identity);
            ProjectileDestruction projScript = projObj.GetComponent<ProjectileDestruction>();
            if (projScript != null) projScript.Setup(dir, ProjectileSpeed, DmgDestruction_Ball, player.userId);
        }
    }
}