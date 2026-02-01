using UnityEngine;

public class Heal : UseEffect
{
    [SerializeField] private float PvHeal = 20f;

    public override void Use()
    {
        // On récupère le script de vie sur le parent au moment de l'utilisation
        Player_Point_De_Vie pv = GetComponentInParent<Player_Point_De_Vie>();
        if (pv != null)
        {
            pv.GetDomage(-PvHeal);
        }
    }
}