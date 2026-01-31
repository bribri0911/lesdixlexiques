using UnityEngine;

public class CooldownObject : MonoBehaviour
{
    public float cooldown = 2f; // durée du cooldown en secondes
    private float nextTimeAvailable = 0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && CanUse())
        {
            UseObject();
        }
    }

    bool CanUse()
    {
        return Time.time >= nextTimeAvailable;
    }

    void UseObject()
    {
        Debug.Log("Objet utilisé !");
        nextTimeAvailable = Time.time + cooldown;
    }
}
