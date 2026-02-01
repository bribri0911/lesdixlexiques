using UnityEngine;

public class GeneratMack : MonoBehaviour
{
    [SerializeField] private GameObject[] GetAllMask;
    [SerializeField] private int numberOfMasks = 10;
    [SerializeField] private bool isGameStarted = false;
    [SerializeField] private GameObject parentOfMask;

    private Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;
        parentOfMask = GameObject.FindGameObjectWithTag("ParentMask");
    }

    void OnEnable()
    {
        WebsocketManage.OnStartGame += HandleActionStartGame;
        WebsocketManage.OnResetGame += HandleActionReset;
    }

    void OnDisable()
    {
        WebsocketManage.OnStartGame -= HandleActionStartGame;
        WebsocketManage.OnResetGame -= HandleActionReset;
    }

    void HandleActionStartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            GenerateMasks();
        }
    }

    void HandleActionReset()
    {
        if (isGameStarted)
        {
            isGameStarted = false;
            DestroyAllMask();
        }
    }

    void GenerateMasks()
    {
        if (GetAllMask == null || GetAllMask.Length == 0)
        {
            Debug.LogError("⚠️ Aucun masque assigné dans GetAllMask !");
            return;
        }

        Vector2 min = mainCam.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
        Vector2 max = mainCam.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));

        for (int i = 0; i < numberOfMasks; i++)
        {
            float randomX = Random.Range(min.x, max.x);
            float randomY = Random.Range(min.y, max.y);
            Vector3 spawnPos = new Vector3(randomX, randomY, 0);

            int randomIndex = Random.Range(0, GetAllMask.Length);
            GameObject prefabToSpawn = GetAllMask[randomIndex];

            GameObject temps = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            temps.transform.parent = parentOfMask.transform;
        }

        Debug.Log($"🎭 {numberOfMasks} masques ont été générés aléatoirement.");
    }

    void DestroyAllMask()
    {
        GameObject[] temps = parentOfMask.GetComponentsInChildren<GameObject>();
        foreach (GameObject item in temps)
        {
            Destroy(item);
        }
    }

}