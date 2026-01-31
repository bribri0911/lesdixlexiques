using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Force l'ajout d'un Rigidbody2D sur le préfab
public class Projectile : MonoBehaviour
{
    public float lifetime = 10f;
    private Rigidbody2D rb;
    private float damage; // Pour stocker les dégâts si besoin plus tard

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0; // S'assurer que le projectile ne tombe pas
        Destroy(gameObject, lifetime); // Auto-destruction
    }

    public void Setup(Vector2 direction, float speed, float dmg)
    {
        damage = dmg;

        // Normaliser la direction pour avoir une vitesse constante peu importe la diagonale
        rb.linearVelocity = direction.normalized * speed;

        // Optionnel : Orienter le sprite dans la direction du tir
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    // Ici vous pourrez ajouter OnTriggerEnter2D pour gérer les collisions
}
