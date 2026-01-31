using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 10f;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(gameObject);
    }
}