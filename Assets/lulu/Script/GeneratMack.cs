using System.Collections.Generic;
using UnityEngine;

public class GeneratMack : MonoBehaviour
{
    public static GeneratMack Instance; // Singleton pour un accès facile
    [SerializeField] private GameObject[] GetAllMask;
    [SerializeField] private int numberOfMasks = 10;
    [SerializeField] private bool isGameStarted = false;
    [SerializeField] public GameObject parentOfMask;

    // Liste de tous les masques actuellement au sol
    public List<GameObject> activeMasksOnFloor = new List<GameObject>();

    private Camera mainCam;

    void Awake() => Instance = this;

    void Start()
    {
        mainCam = Camera.main;
        if (parentOfMask == null) parentOfMask = GameObject.FindGameObjectWithTag("ParentMask");
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
        isGameStarted = false;
        DestroyAllMask();
    }

    void GenerateMasks()
    {
        if (GetAllMask == null || GetAllMask.Length == 0) return;

        Vector2 min = mainCam.ViewportToWorldPoint(new Vector2(0.1f, 0.1f));
        Vector2 max = mainCam.ViewportToWorldPoint(new Vector2(0.9f, 0.9f));

        for (int i = 0; i < numberOfMasks; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(min.x, max.x), Random.Range(min.y, max.y), 0);
            GameObject prefabToSpawn = GetAllMask[Random.Range(0, GetAllMask.Length)];

            GameObject temps = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            temps.transform.parent = parentOfMask.transform;
            
            activeMasksOnFloor.Add(temps); // On enregistre le masque
        }
    }

    void DestroyAllMask()
    {
        foreach (GameObject item in activeMasksOnFloor)
        {
            if (item != null) Destroy(item);
        }
        activeMasksOnFloor.Clear();
    }
}