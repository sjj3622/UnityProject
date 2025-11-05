using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HeartSpeedController : MonoBehaviour
{
    GameManager gameManager;
    public GameObject Panel;

    private Animator animator;

    [Header("Speed Settings")]
    public float minSpeed = 0f;
    public float maxSpeed = 5.0f;
    public float speedIncreaseRate = 0.8f;
    public float speedDecreaseRate = 0.01f;

    [Header("Stage Settings")]
    public int currentStage = 1; // 1~3단계
    private bool isCollidingWithHandle = false;

    private KeyCode targetKey; // 3단계 랜덤 키
    private float randomKeyTimer = 0f;
    public float randomKeyInterval = 3f; // 랜덤 키 갱신 간격
    //public static string gameState;


    [SerializeField] Text messageText;

    void Start()
    {
        //if (Panel != null)
        //    Panel.SetActive(false);
        gameManager = FindObjectOfType<GameManager>();

        animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = minSpeed;
    }

    void Update()
    {
        if (animator == null) return;

        Debug.Log("currentStage" + currentStage);

        switch (currentStage)
        {
            case 1:
                Stage1Logic();
                break;
            case 2:
                Debug.Log("2단계시작");
                Stage2Logic();
                break;

            case 3:
                Stage3Logic();
                break;
        }

        // 최대속도 제한
        animator.speed = Mathf.Clamp(animator.speed, 0f, maxSpeed);

        // 최댓값 도달 시 
        if (animator.speed >= maxSpeed)
        {
            GameManager.gameState = "StageClear";
            SceneManager.LoadScene("CPR1");
        }
    }
    void Stage1Logic()
    {
        if (isCollidingWithHandle)
        {
            messageText.text = "1단계 시작!";
            Debug.Log("animator.speed" + animator.speed);

            if (Input.GetKey(KeyCode.Space) && isCollidingWithHandle == true)
                animator.speed += speedIncreaseRate * Time.deltaTime;
            else
                animator.speed -= speedDecreaseRate * Time.deltaTime;
        }
        //else
        //{
        //    //충돌 안 할 때는 천천히 감소
        //    animator.speed -= (speedDecreaseRate / 2) * Time.deltaTime;
        //}

        if (animator.speed >= 1.5f && animator.speed < 3.0f)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "1단계 성공!";
            StartCoroutine(NextStageDelay(2, 1.5f)); // 1단계 → 2단계로 1.5초 대기 후 이동

        }
    }


    void Stage2Logic()
    {

        
        if (isCollidingWithHandle)
        {
            messageText.text = "2단계 시작!";
            Debug.Log("animator.speed" + animator.speed);

            if (Input.GetKey(KeyCode.Space) && isCollidingWithHandle == true)
                animator.speed += speedIncreaseRate * Time.deltaTime;
            else
                animator.speed -= speedDecreaseRate;
        }
        else
        {
            //충돌 안 할 때는 천천히 감소
            animator.speed -= (speedDecreaseRate / 2) * Time.deltaTime;
        }

        if(animator.speed >= 3.0f && animator.speed < 5.0f)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "2단계 성공!";
            StartCoroutine(NextStageDelay(3, 1.5f)); // 2단계 → 3단계로 1.5초 대기 후 이동
            
        }

    }

    void Stage3Logic()
    {
        randomKeyTimer += Time.deltaTime;

        // 일정 시간마다 새로운 랜덤 키 생성
        if (randomKeyTimer >= randomKeyInterval || targetKey == KeyCode.None || !isCollidingWithHandle)
        {
            Debug.Log("animator.speed" + animator.speed);

            randomKeyTimer = 0f;
            targetKey = (KeyCode)Random.Range((int)KeyCode.A, (int)KeyCode.B + 1);
            Debug.Log($"새 랜덤 키: {targetKey}");
            messageText.text = $"눌러야 할 키: {targetKey}";
        }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(targetKey))
            {
                animator.speed += speedIncreaseRate * Time.deltaTime * 10f; // 반응 빠르게 증가
                Debug.Log("정확한 키 입력!");
                targetKey = KeyCode.None; // 다음 키로 넘어감
            }
            else
            {
                animator.speed -= 0.01f;
                Debug.Log("틀린 키 입력!");
            }

        }
            if(animator.speed >= 5.0f)
            {
                animator.speed = maxSpeed;
                messageText.text = "GameClaer";
            }
        
    }


    // Handle과 충돌 감지
    void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.gameObject.CompareTag("Handle"))
        {

            isCollidingWithHandle = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
        {

            isCollidingWithHandle = false;
        }
    }




    IEnumerator NextStageDelay(int nextStage, float delay)
    {
        currentStage = 0; // 잠시 Stage 진행 멈춤
        yield return new WaitForSeconds(delay);

        messageText.text = $"{nextStage}단계 시작!";
        currentStage = nextStage;
    }
}
