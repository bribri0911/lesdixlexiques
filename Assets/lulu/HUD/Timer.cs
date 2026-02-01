using TMPro;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMesh;
    private float currentTime = 0f;
    private bool isGameStarted = false; 

    [SerializeField] private float COUNTDOWN_START = 30f;
    
    private Vector3 originalPosition;
    private float originalFontSize;

    void Start()
    {
        originalPosition = textMesh.transform.localPosition;
        originalFontSize = textMesh.fontSize;
        
        ResetTimerValues();
    }

    void OnEnable()
    {
        WebsocketManage.OnStartGame += HandleActionStartGame;
        WebsocketManage.OnResetGame += HandleRestartGame;
    }

    void OnDisable()
    {
        WebsocketManage.OnStartGame -= HandleActionStartGame;
        WebsocketManage.OnResetGame -= HandleRestartGame;
    }

    void HandleActionStartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            currentTime = 0f; 
        }
    }

    void HandleRestartGame()
    {
        ResetTimerValues();
    }

    private void ResetTimerValues()
    {
        isGameStarted = false;
        currentTime = 0f;
        
        textMesh.text = "READY?";
        textMesh.color = Color.white;
        textMesh.fontSize = originalFontSize;
        textMesh.transform.localPosition = originalPosition;
    }

    void Update()
    {
        if (!isGameStarted) return;

        currentTime += Time.deltaTime;

        if (currentTime < COUNTDOWN_START)
        {
            HandleCountdown(COUNTDOWN_START - currentTime);
        }
        else
        {
            HandleStopwatch(currentTime - COUNTDOWN_START);
        }
    }

    private void HandleCountdown(float remainingTime)
    {
        int seconds = (int)remainingTime;
        int milliseconds = (int)((remainingTime - seconds) * 100);
        textMesh.text = $"{seconds:00}:{milliseconds:00}";

        if (remainingTime <= 5f)
        {
            textMesh.color = Color.red;
            float t = 1f - (remainingTime / 5f); 
            textMesh.fontSize = Mathf.Lerp(originalFontSize, originalFontSize * 1.5f, t);
            float shakeIntensity = t * 5f; 
            textMesh.transform.localPosition = originalPosition + (Vector3)Random.insideUnitCircle * shakeIntensity;
        }
        else
        {
            textMesh.color = Color.white;
            textMesh.fontSize = originalFontSize;
            textMesh.transform.localPosition = originalPosition;
        }
    }

    private void HandleStopwatch(float elapsedTime)
    {
        if (textMesh.color != Color.white || textMesh.fontSize != originalFontSize)
        {
            textMesh.color = Color.white;
            textMesh.fontSize = originalFontSize;
            textMesh.transform.localPosition = originalPosition;
        }

        int minutes = (int)(elapsedTime / 60);
        int seconds = (int)(elapsedTime % 60);
        int milliseconds = (int)((elapsedTime - (int)elapsedTime) * 100);

        textMesh.text = $"{minutes:00}:{seconds:00}:{milliseconds:00}";
    }
}