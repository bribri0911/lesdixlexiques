using UnityEngine;

public class Vitesse : MonoBehaviour
{
    [SerializeField] private float rapidity = 5f;
    private PlayerController2D player;
    void OnEnable()
    {
         player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            player.moveSpeed = player.movespeedDeafal * rapidity;
        }
    }
     void OnDisable()
    {
        if (player != null)
        { 
            player.moveSpeed = player.movespeedDeafal;
        }
    }
}
