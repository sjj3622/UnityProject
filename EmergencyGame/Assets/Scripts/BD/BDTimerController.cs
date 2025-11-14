using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BDTimerController : MonoBehaviour
{
    public Text timerText;
    public float timerDuration = 180f;
    public float timer;

    public bool timerRunning = false;

    public static BDTimerController Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        timerText.text = "";
        timer = timerDuration;
    }

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

    // 씬 전환 함수
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

    // 실행 여부 설정
    public void SetTimerRunning(bool value) => timerRunning = value;
}
