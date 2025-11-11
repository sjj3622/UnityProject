using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HeartSpeedControllerOne : MonoBehaviour
{
    GameManager gameManager;

    private Animator animator;
    private BoxCollider2D boxCollider;
    TimerController timerController;

    [Header("Speed Settings")]
    public float minSpeed = 0f;
    public float maxSpeed = 5.0f;
    public float speedIncreaseRate = 0.8f;
    public float speedDecreaseRate = 0.01f;
    float score = 0;

    [Header("Stage Settings")]
    public int currentStage = 1; // 1~3 단계
    private bool isCollidingWithHandle = false;
    private KeyCode targetKey;
    private float randomKeyTimer = 0f;
    public float randomKeyInterval = 5f;

    [SerializeField] Text messageText;
    bool stageStarted = false;

    // 원래 콜라이더 크기 저장용
    private Vector2 originalColliderSize;
    private bool colliderChanged = false;

    //  하락 확률 관련 설정
    [Header("Stage Downgrade Settings")]
    [Range(0f, 1f)] public float fallChance = 0.1f; // 10% 확률로 하락
    public float downgradeDelay = 1.2f;

    
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = minSpeed;

        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
            originalColliderSize = boxCollider.size;

        timerController = GetComponent<TimerController>();

        
        
        
    }

    void Update()
    {

       
        if (GameManager.gameState != "gameStart") return;
        if (animator == null) return;

        switch (currentStage)
        {
            case 1:
                Stage1Logic();
                break;
            case 2:
                Stage2Logic();
                break;
            case 3:
                Stage3Logic();
                break;
        }

        animator.speed = Mathf.Clamp(animator.speed, 0f, maxSpeed);

        if (score >= 100)
        {
            animator.speed = maxSpeed;
            GameManager.gameState = "StageClear";
            Debug.Log("게임스테이지"+GameManager.gameState);
            SceneStateManager.instance.SaveState(GameObject.Find("Timer"));
            SceneManager.LoadScene("CPR");
        }
    }

    void Stage1Logic()
    {
        if (!stageStarted)
        {
            messageText.text = "1단계 시작!";
            stageStarted = true;
        }

        if (isCollidingWithHandle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.speed += speedIncreaseRate;
                score += 100;
                isCollidingWithHandle = false;
                UpdateScoreAndStageMessage();
            }
        }

        if (score >= 5)
        {
            messageText.text = "1단계 성공!";
            StartCoroutine(NextStageDelay(2, 1.5f));
        }
    }

    void Stage2Logic()
    {
        if (!colliderChanged && boxCollider != null)
        {
            Vector2 newSize = boxCollider.size;
            newSize.x = 4f;
            boxCollider.size = newSize;
            colliderChanged = true;
        }

        if (isCollidingWithHandle)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.speed += speedIncreaseRate;
                score += 5;
                isCollidingWithHandle = false;
                UpdateScoreAndStageMessage();

                //  일정 확률로 하락 시도
                TryStageDowngrade();
            }
        }
        else if (!isCollidingWithHandle && Input.GetKeyDown(KeyCode.Space) && score > 5)
        {
            score -= 1;
            UpdateScoreAndStageMessage();

            // 실수 시 하락 확률 증가
            TryStageDowngrade();
        }

        if (score > 50 && boxCollider != null && colliderChanged)
        {
            boxCollider.size = originalColliderSize;
            colliderChanged = false;

            messageText.text = "2단계 성공!";
            StartCoroutine(NextStageDelay(3, 1.5f));
        }
    }

    void Stage3Logic()
    {
        randomKeyTimer += Time.deltaTime;

        if (randomKeyTimer >= randomKeyInterval || targetKey == KeyCode.None || !isCollidingWithHandle)
        {
            randomKeyTimer = 0f;
            KeyCode[] possibleKeys = { KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.F };
            targetKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
            UpdateScoreAndStageMessage();
        }

        if (isCollidingWithHandle && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(targetKey))
            {
                animator.speed += speedIncreaseRate;
                targetKey = KeyCode.None;
                score += 10;
                UpdateScoreAndStageMessage();

                //  올바른 입력 후에도 낮은 확률로 하락 가능
                TryStageDowngrade();
            }
            else
            {
                score -= 2;
                UpdateScoreAndStageMessage();

                //  틀린 입력일 때 하락 시도
                TryStageDowngrade();
            }
        }
    }

    //  확률적으로 단계 하락 메서드
    void TryStageDowngrade()
    {
        if (currentStage <= 1) return; // 1단계 이하로는 안 내려감
        float randomValue = Random.value; // 0~1 사이 난수
        if (randomValue < fallChance)
        {
            int downgradedStage = currentStage - 1;
            StartCoroutine(DowngradeStage(downgradedStage));
        }
    }

    IEnumerator DowngradeStage(int newStage)
    {
        messageText.text = $" 실수로 {currentStage}단계에서 {newStage}단계로 하락!";
        yield return new WaitForSeconds(downgradeDelay);

        //  하락 시 속도 절반으로 감소
        animator.speed /= 2f;

        // 단계별 점수 초기화 조건
        if (currentStage == 3 && newStage == 2)
        {
            score = 10;
        }
        else if (currentStage == 2 && newStage == 1)
        {
            score = 0;
        }

        currentStage = newStage;
        messageText.text = $"{newStage}단계 다시 도전!";
        UpdateScoreAndStageMessage();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
            isCollidingWithHandle = true;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
            isCollidingWithHandle = false;
    }

    void UpdateScoreAndStageMessage()
    {
        if (currentStage <= 2)
        {
            messageText.text = $"점수: {score}\n현재 단계: {currentStage}";
        }
        else
        {
            messageText.text = $"점수: {score}\n현재 단계: {currentStage}\n눌러야할 키: {targetKey}";
        }
    }

    IEnumerator NextStageDelay(int nextStage, float delay)
    {
        currentStage = 0;
        yield return new WaitForSeconds(delay);
        messageText.text = $"{nextStage}단계 시작!";
        currentStage = nextStage;
    }
}
