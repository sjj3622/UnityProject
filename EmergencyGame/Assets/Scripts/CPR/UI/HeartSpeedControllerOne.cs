using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class HeartSpeedControllerOne : MonoBehaviour
{
    GameManager gameManager;
    public GameObject Panel;

    private Animator animator;

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

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        animator = GetComponent<Animator>();
        if (animator != null)
            animator.speed = minSpeed;
    }

    void Update()
    {

        Debug.Log("스코어 :" + score);
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
            SceneManager.LoadScene("CPR1");
        }
    }

    void Stage1Logic()
    {
        if (isCollidingWithHandle)
        {
            messageText.text = "1단계 시작!";

           
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.speed += speedIncreaseRate;
                score += 1;
            }
            //else { score -= 1; }
        }

       


        if (score > 5)
        {
            messageText.gameObject.SetActive(true);
            messageText.text = "1단계 성공!";
            StartCoroutine(NextStageDelay(2, 1.5f));
        }
    }

    void Stage2Logic()
    {
        if (isCollidingWithHandle)
        {
            

            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.speed += speedIncreaseRate;
                score += 5;
            }
            //else { score -= 1; }
        }


        if (score > 50)
        {
            messageText.gameObject.SetActive(true);
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

            messageText.text = $"눌러야 할 키: {targetKey}";
        }

        if (isCollidingWithHandle && Input.anyKeyDown)
        {
            if (Input.GetKeyDown(targetKey))
            {
                animator.speed += speedIncreaseRate;
                targetKey = KeyCode.None;
                messageText.text = "좋습니다!";
                score += 10;
            }
            else
            {
                score -= 2;
                messageText.text = "아닙니다!";
            }
        }

        //if (score == 100)
        //{
        //    animator.speed = maxSpeed;
        //    messageText.text = "Game Clear!";
        //}
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

    IEnumerator NextStageDelay(int nextStage, float delay)
    {
        currentStage = 0;
        yield return new WaitForSeconds(delay);

        messageText.text = $"{nextStage}단계 시작!";
        currentStage = nextStage;
    }
}
