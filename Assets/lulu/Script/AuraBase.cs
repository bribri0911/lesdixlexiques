using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))] // Force l'ajout d'un Rigidbody2D sur le préfab
public class AuraBase  : MonoBehaviour
{
    private float damagePerSecond;
    private float tickTimer = 0f;
    private float tickInterval = 0.5f; // Dégâts toutes les 0.5 secondes

    public void Setup(float duration, float dmg)
    {
        this.damagePerSecond = dmg;
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        // 1. SÉCURITÉ : On vérifie que ce n'est pas le joueur lui-même
        // (L'aura est enfant du joueur, donc transform.parent = le joueur)
        if (transform.parent != null && other.gameObject == transform.parent.gameObject)
            return;

        // 2. On vérifie si la cible a des PV (Joueur ou Ennemi)
        // J'utilise TryGetComponent pour éviter les erreurs si l'objet n'a pas de PV
        if (other.TryGetComponent<Player_Point_De_Vie>(out Player_Point_De_Vie targetHealth))
        {
            // Gestion du temps (Timer)
            tickTimer += Time.deltaTime;

            if (tickTimer >= tickInterval)
            {
                // 3. APPLICATION DES DÉGÂTS via votre fonction
                Debug.Log($"[Aura] {name} brûle {other.name} pour {damagePerSecond} dégâts");
                
                targetHealth.GetDomage(damagePerSecond);

                // Reset du timer
                tickTimer -= tickInterval;
            }
        }
    }

    
}