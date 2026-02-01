using UnityEngine;

public class Beton : UseEffect
{
    [SerializeField] private float lifeTime = 5f;
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private GameObject gOwall;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null && gOwall != null)
        {
            Vector2 dir = player.lastDirection == Vector2.zero ? Vector2.down : player.lastDirection.normalized; 
            Vector3 pos = player.transform.position + (Vector3)(dir * spawnDistance);
            Destroy(Instantiate(gOwall, pos, Quaternion.identity), lifeTime);
        }
    }
}