using TMPro;
using UnityEngine;

public class CountDeath : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI countPlayerAlive;


    void OnEnable()
    {
        FactoryManager.OnActionInPlayerInGame += HandlePlayerInGame;
    }

    void OnDisable()
    {
        FactoryManager.OnActionInPlayerInGame -= HandlePlayerInGame;
    }

    void HandlePlayerInGame(int nbrUserAlive)
    {
        countPlayerAlive.text = $"{nbrUserAlive} Joueurs";
    }

}
