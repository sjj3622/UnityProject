using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class BurnTimerController : MonoBehaviour
{
    public static BurnTimerController Instance;

    [Header("타이머 UI (씬마다 달라질 수 있음)")]
    public TextMeshProUGUI timerText;

    [Header("설정")]
    public float timerDuration = 180f;
    public float timer;
    public bool timerRunning = false;
    public float totalTimer;

    // UI를 찾는 데 사용할 이름 또는 태그 (프로젝트에 맞게 설정)
    public string timerTextObjectName = "TImerTEXT";        // GameObject 이름으로 찾기
    public string timerTextTag = "TimerTextUI";            // 또는 태그로 찾기 (태그가 있으면 우선 사용)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            transform.SetParent(null);      // 안전하게 루트로 분리
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        timer = timerDuration;
        timerText = FindTimerTextImmediate(); // 현재 씬에 이미 UI가 있으면 연결
        UpdateTimerText();
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 로드 직후에 UI가 아직 생기지 않거나 동적으로 만들어지는 경우가 있어
        // 단순한 Find로 못 찾으면 코루틴으로 잠시 기다려서 찾아본다.
        timerText = FindTimerTextImmediate();
        if (timerText == null)
        {
            // 최대 대기 프레임 수나 타임아웃을 지정해 안전하게 반복
            StartCoroutine(WaitAndFindTimerText(60)); // 최대 60프레임 대기
        }
        else
        {
            UpdateTimerText();
        }
    }

    TextMeshProUGUI FindTimerTextImmediate()
    {
        // 태그가 있으면 태그 우선
        if (!string.IsNullOrEmpty(timerTextTag))
        {
            var tagged = GameObject.FindWithTag(timerTextTag);
            if (tagged != null)
            {
                var t = tagged.GetComponent<TextMeshProUGUI>();
                if (t != null) return t;
                // 태그가 부모 오브젝트라면 자식에서 찾기
                t = tagged.GetComponentInChildren<TextMeshProUGUI>();
                if (t != null) return t;
            }
        }

        // 이름으로 찾기
        if (!string.IsNullOrEmpty(timerTextObjectName))
        {
            var go = GameObject.Find(timerTextObjectName);
            if (go != null)
            {
                var t = go.GetComponent<TextMeshProUGUI>();
                if (t != null) return t;
                t = go.GetComponentInChildren<TextMeshProUGUI>();
                if (t != null) return t;
            }
        }

        // 씬 전체에서 첫 TMPUGUI 찾기(최후의 수단)
        var all = FindObjectsOfType<TextMeshProUGUI>();
        if (all != null && all.Length > 0) return all[0];

        return null;
    }

    IEnumerator WaitAndFindTimerText(int maxFrames)
    {
        int count = 0;
        while (count < maxFrames)
        {
            var t = FindTimerTextImmediate();
            if (t != null)
            {
                timerText = t;
                UpdateTimerText();
                yield break;
            }
            count++;
            yield return null; // 한 프레임 대기
        }

        // 못 찾았으면 그냥 종료(다음 씬에서 다시 시도)
        yield break;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Destroy(gameObject);
        }

        if(BurngpManager.gameState == null)
        {
            timer = timerDuration;
            timerDuration = 180f;
        }
        // 예시: F6로 시간 단축 (테스트용)
        if (Input.GetKeyDown(KeyCode.F6))
        {
            timer -= 20f;
            UpdateTimerText();
        }

        if ((BurngpManager.gameState == "Rescuer" || BurngpManager.gameState == "FireFighter") && !timerRunning)
            timerRunning = true;

        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                timerRunning = false;
                BurngpManager.gameState = "BOver";
            }
            UpdateTimerText();
        }

        if (BurngpManager.gameState == "RescuerClear" || BurngpManager.gameState == "FireFighterClear")
        {
            timerRunning = false;
            totalTimer = timer;
            UpdateTimerText();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            if (BurngpManager.gameState == null)
            {
                timerText.text = "";
            }
            else
            {
                int minutes = Mathf.FloorToInt(timer / 60f);
                int seconds = Mathf.FloorToInt(timer % 60f);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }
    }

    // 외부에서 시간 설정/얻기 가능
    public float GetCurrentTime() => timer;
    public void SetCurrentTime(float value)
    {
        timer = value;
        UpdateTimerText();
    }
}
