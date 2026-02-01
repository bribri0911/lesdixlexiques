using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    public string userId;

    [Header("Vitesse")]
    public float moveSpeed = 5f;
    public float movespeedDeafal = 5f;

    [Header("Réglages Glisse")]
    public float acceleration = 10f;
    public float deceleration = 2f;

    [Header("Détection Masques")]
    [SerializeField] private float detectionRadius = 2.5f; // Rayon de ramassage
    [SerializeField] private LayerMask maskLayer;        // Layer à configurer dans Unity

    public Vector2 lastDirection;
    private Rigidbody2D rb;
    private Vector2 targetVelocity;

    [SerializeField] private GetMask getMask;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        moveSpeed = movespeedDeafal;
    }

    // --- MOUVEMENT ---

    public void Move(Vector2 direction)
    {
        lastDirection = direction;
        targetVelocity = direction;
    }

    public void SetMovementState(float accel, float decel)
    {
        acceleration = accel;
        deceleration = decel;
    }

    public void ModifMovement(float TimeSlow)
    {
        StopAllCoroutines(); // Sécurité pour éviter les conflits de vitesse
        StartCoroutine(ResetSpeedRoutine(TimeSlow));
        moveSpeed = 0f;
    }

    private System.Collections.IEnumerator ResetSpeedRoutine(float TimeSlow)
    {
        yield return new WaitForSeconds(TimeSlow);
        moveSpeed = movespeedDeafal;
    }

    public void ResetMovement()
    {
        acceleration = 10f;
        deceleration = 5f;
        moveSpeed = movespeedDeafal;
    }

    // --- ACTIONS MASQUES ---

    public void UseMask()
    {
        if (getMask != null) getMask.UseMaskActive();
    }

    public void ChangeMask(int direction)
    {
        if (getMask != null) getMask.ChangeMask(direction);
    }

    public void RemoveAllMask()
    {
        if (getMask == null) return;
        for (int i = 0; i < getMask.nbrMaskMax; i++)
        {
            getMask.RemoveMask(0);
        }
    }

    public void GetMaskFunction()
    {
        if (getMask == null || GeneratMack.Instance == null) return;

        GameObject closestMask = null;
        float minDistance = detectionRadius; 

        for (int i = GeneratMack.Instance.activeMasksOnFloor.Count - 1; i >= 0; i--)
        {
            GameObject mask = GeneratMack.Instance.activeMasksOnFloor[i];
            if (mask == null) continue;

            float dist = Vector2.Distance(transform.position, mask.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closestMask = mask;
            }
        }

        if (closestMask != null)
        {
            closestMask.transform.position = new Vector3(0,0, 0);
            getMask.AddMask(closestMask);
            GeneratMack.Instance.activeMasksOnFloor.Remove(closestMask);
            Debug.Log($"🎭 Masque ramassé (Distance) : {closestMask.name} - {closestMask.transform.position}");
        }
    }


    void FixedUpdate()
    {
        // Application du mouvement avec Lerp pour l'effet de glisse/inertie
        Vector2 currentTargetVel = targetVelocity * moveSpeed;
        float lerpSpeed = (targetVelocity != Vector2.zero) ? acceleration : deceleration;

        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, currentTargetVel, lerpSpeed * Time.fixedDeltaTime);

        // On reset la direction cible pour le prochain frame (attente d'un nouvel input)
        targetVelocity = Vector2.zero;
    }

}