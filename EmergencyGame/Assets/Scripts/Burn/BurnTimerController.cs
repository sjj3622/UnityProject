using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class BurnTimerController : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float timerDuration = 300f;
    public float totalTimer;

    public float timer;
    public bool timerRunning = false;

    void Start()
    {
        timerText.text = "";
        timer = timerDuration;
    }

    // Update is called once per frame
    void Update()
    {

        // 게임 시작 시 실행
        if (BDgpManager.gameState == "BDStart" && !timerRunning)
        {
            timerRunning = true;

        }

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
            }


            UpdateTimerText();
        }
        if (BDgpManager.gameState == "BDClear")
        {
            timerRunning = false;

            totalTimer = timer;
        }

    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
    public void GoToNextScene(string sceneName)
    {
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
        SceneManager.LoadScene(sceneName);
    }

    // 현재 시간 얻기
    public float GetCurrentTime() => timer;

    // 현재 시간 설정
    public void SetCurrentTime(float value)
    {
        timer = value;
        UpdateTimerText();
    }
}
