using UnityEngine;

public class Destruction_Ball : UseEffect
{
    [SerializeField]
    private float DmgDestruction_Ball = 50f;

    [SerializeField]
    private float ProjectileSpeed = 15f;

    [SerializeField]
    private GameObject gODestruction_Ball;

    [SerializeField]
    private float SelfStunTime = 1.5f;
    public override void Use()
    {

        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            player.ModifMovement(SelfStunTime);

            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.down : player.lastDirection;
            dir.Normalize();
            Vector3 spawnPos = player.transform.position + (Vector3)(dir * 1.0f); 

            GameObject projObj = Instantiate(gODestruction_Ball, transform.position, Quaternion.identity);

            projObj.transform.parent = player.transform;

           ProjectileDestruction projScript = projObj.GetComponent<ProjectileDestruction>();

            
            Vector2 dir2 = player.lastDirection == Vector2.zero ? Vector2.right : player.lastDirection;

            if (projScript != null)
            {
                projScript.Setup(dir2, ProjectileSpeed, DmgDestruction_Ball, player.userId);
            }
        }
        else
        {
            Debug.LogWarning("PlayerController2D introuvable dans les parents de Destruction_Ball !");
        }
    }


}
