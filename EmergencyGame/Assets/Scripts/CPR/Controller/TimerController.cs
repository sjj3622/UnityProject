using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerController : MonoBehaviour
{
    public GameObject Player;
    public GameObject Patient;
    [Header("Timer Settings")]
    public Text timerText;
    public float timerDuration = 180f;
    public float totalTimer = 0f;


    private float timer;
    private bool timerRunning = false;

    void Awake()
    {
        //if (transform.parent != null)
        //    transform.SetParent(null); // 부모에서 분리해서 루트로 이동

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

    }

    void Update()
    {

        //Debug.Log(gameObject.name + " 위치: " + transform.position +
        //      ", 활성화: " + gameObject.activeSelf);

        if (GameManager.gameState == "StageClear")
        {
            timerRunning = false;

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

        timerRunning = true;
    }
    public void StartTimerDirectly()
    {
        if (!timerRunning)
        {

            timer = timerDuration;
            timerRunning = true;
            Debug.Log("타이머 직접 시작됨");
        }

    }
    public void TimerSave()
    {
        if (GameManager.gameState == "StageRule")
        {
            Debug.Log("타이머 2");
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
            Debug.Log("타이머 3");
            
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
