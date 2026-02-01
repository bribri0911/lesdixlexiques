using UnityEngine;

public class Vitesse : MonoBehaviour
{
    private PlayerController2D player;
    void OnEnable()
    {
         player = GetComponentInParent<PlayerController2D>();

        if (player != null)
        {
            player.moveSpeed = player.movespeedDeafal * 2f;
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
