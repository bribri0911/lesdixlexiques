using TMPro;
using UnityEngine;

public class CountDeath : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countPlayerAlive;
    [SerializeField] private Canvas canvasVictory;

    void Awake()
    {
        if (canvasVictory != null)
        {
            canvasVictory.enabled = false;
        }
    }

    void OnEnable()
    {
        FactoryManager.OnActionInPlayerInGame += HandlePlayerInGame;
        FactoryManager.OnWinGame += HandleWinGame;
        WebsocketManage.OnResetGame += HandleResetGame;
    }

    void OnDisable()
    {
        FactoryManager.OnActionInPlayerInGame -= HandlePlayerInGame;
        FactoryManager.OnWinGame -= HandleWinGame;
        WebsocketManage.OnResetGame -= HandleResetGame;
    }

    void HandlePlayerInGame(int nbrUserAlive)
    {
        countPlayerAlive.text = $"{nbrUserAlive} Joueurs";
    }

    void HandleWinGame()
    {
        if (canvasVictory != null)
        {
            canvasVictory.enabled = true;
            
            canvasVictory.renderMode = RenderMode.ScreenSpaceOverlay;
            
            canvasVictory.sortingOrder = 999; 
            
        }
    }

    void HandleResetGame()
    {
        if (canvasVictory != null)
        {
            canvasVictory.enabled = false;            
        }
    }
}