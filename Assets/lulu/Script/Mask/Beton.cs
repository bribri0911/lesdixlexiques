using UnityEngine;
public class Beton : UseEffect
{
    [SerializeField]
    private float lifeTime = 5000f;

    [SerializeField]
    private float spawnDistance = 2f;
    
    [SerializeField]
    private GameObject gOwall;

    public override void Use()
    {
        PlayerController2D player = GetComponentInParent<PlayerController2D>();

        if (player != null && gOwall != null)
        {
            Vector2 spawnDir = player.lastDirection;
            
            if (spawnDir == Vector2.zero) spawnDir = Vector2.down; 
            
            spawnDir.Normalize(); 

            Vector3 spawnPosition = player.transform.position + (Vector3)(spawnDir * spawnDistance);

            GameObject gOWallInstance = Instantiate(gOwall, spawnPosition, Quaternion.identity);

            Destroy(gOWallInstance, lifeTime);
        }
    }
}
