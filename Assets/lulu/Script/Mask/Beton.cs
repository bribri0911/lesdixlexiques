using UnityEngine;

public class Beton : UseEffect
{
    [SerializeField] private float lifeTime = 5f; // Réduit à 5s (5000f était immense)
    [SerializeField] private float spawnDistance = 2f;
    [SerializeField] private GameObject gOwall;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();
        if (player != null && gOwall != null)
        {
            Vector2 spawnDir = player.lastDirection == Vector2.zero ? Vector2.down : player.lastDirection.normalized; 
            Vector3 spawnPosition = player.transform.position + (Vector3)(spawnDir * spawnDistance);

            GameObject wall = Instantiate(gOwall, spawnPosition, Quaternion.identity);
            Destroy(wall, lifeTime);
        }
    }
}