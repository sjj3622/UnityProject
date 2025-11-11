using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Patient;
    [Header("Timer Settings")]
    public Text timerText;
    public float timerDuration = 300f;
    public float totalTimer = 0f;


    private float timer;
    private bool timerRunning = false;

    void Awake()
    {
        DontDestroyOnLoad(gameObject); // 씬 전환 시 유지
    }


    void Start()
    {
        if (SceneStateManager.instance != null && !string.IsNullOrEmpty(SceneStateManager.instance.savedTimerText))
        {
            timerText.text = SceneStateManager.instance.savedTimerText;
        }
        else
        {
            timerText.text = "";
        }
        Debug.Log("시작");
    }

    void Update()
    {

        Debug.Log(gameObject.name + " 위치: " + transform.position +
              ", 활성화: " + gameObject.activeSelf);

        if (GameManager.gameState == "StageClear")
        {
            timerRunning = true;
            Debug.Log("timerRunning:" + timerRunning);
        }

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                timerRunning = false;
            }

            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
        

    }



    public void TR()
    {
        Debug.Log("TR시작");
        timerRunning = true;
    }
    public void StartTimerDirectly()
    {
        if (!timerRunning)
        {
            Debug.Log("1번 시작");
            timer = timerDuration;
            timerRunning = true;
            Debug.Log("타이머 직접 시작됨");
        }
        if (GameManager.gameState == "StageRule")
        {
            Debug.Log("2번 시작");
            string savedText = SceneStateManager.instance.savedTimerText;
            if (!string.IsNullOrEmpty(savedText))
            {
                string[] split = savedText.Split(':');
                if (split.Length == 2)
                {
                    int minutes, seconds;
                    if (int.TryParse(split[0], out minutes) && int.TryParse(split[1], out seconds))
                    {
                        timer = minutes * 60 + seconds;
                        timerRunning = true;
                        Debug.Log("저장된 Timer 값으로 재시작: " + timer);
                    }
                }
            }
        }
        if (GameManager.gameState == "StageClear")
        {
            string savedText = SceneStateManager.instance.savedTimerText;
            if (!string.IsNullOrEmpty(savedText))
            {
                string[] split = savedText.Split(':');
                if (split.Length == 2)
                {
                    int minutes, seconds;
                    if (int.TryParse(split[0], out minutes) && int.TryParse(split[1], out seconds))
                    {
                        timer = minutes * 60 + seconds;
                        timerRunning = false;
                        Debug.Log("저장된 Timer 값으로 재시작: " + timer);
                    }
                    totalTimer = timer;
                }
            }
            
        }

        


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!timerRunning &&
            ((collision.CompareTag("Patient") && gameObject.CompareTag("Player")) ||
             (collision.CompareTag("Player") && gameObject.CompareTag("Patient"))))
        {
            StartCoroutine(StartTimer());
        }
    }

    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f);
        timer = timerDuration;
        timerRunning = true;
        Debug.Log("타이머 시작");
    }
}
