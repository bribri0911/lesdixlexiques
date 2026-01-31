using UnityEngine;

public class Heal : UseEffect
{
    [SerializeField]
    private Player_Point_De_Vie player_Point_De_Vie;
    [SerializeField]
    private float PvHeal = 20f;

    void Start()
    {
        player_Point_De_Vie = gameObject.GetComponentInParent<Player_Point_De_Vie>();
    }

    public override void Use()
    {
        player_Point_De_Vie.GetDomage(-PvHeal);
    }
}
