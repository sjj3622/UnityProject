using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI Text 사용 위해 추가

public class patientController : MonoBehaviour
{
    public Transform tr;
    Rigidbody2D rb;
    Animator animator;

    [Header("Animation Names")]
    public string RunAni = "SantaIdle";
    public string DeadAni = "SantaDead";
    public string StataAni = "Stata";

    // string nowAni = "", oldAni = "";

    [Header("Move Settings")]
    public float speed = 2f; // 이동 속도
    public float targetX = -3f; // 도착 지점 X좌표

    [Header("Timer Settings")]
    public Text timerText; // UI Text 연결 (인스펙터에서 넣기)
    public float timerDuration = 300f; // 5분 = 300초
    private float timer;
    private bool timerRunning = false;

    bool isDead = false;
    public bool isCarried = false; // Medic이 태웠는지 여부



    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tr = transform;

        animator.Play(RunAni);

        if (timerText != null)
            timerText.text = ""; // 시작 시 빈 상태
    }

    void Update()
    {
        if (!isDead && !isCarried) // Medic이 태운 상태라면 이동 정지
        {
            // 이동
            if (tr.position.x > targetX)
            {
                tr.Translate(Vector3.left * speed * Time.deltaTime);
                animator.Play(RunAni);
            }
            else
            {
                // 도착 시
                isDead = true;
                animator.Play(DeadAni);
                StartCoroutine(StartTimer());
            }
        }

        // 타이머가 켜져 있으면 업데이트
        if (timerRunning)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                timer = 0;
                timerRunning = false;
            }

            // 남은 시간 텍스트 표시
            if (timerText != null)
            {
                int minutes = Mathf.FloorToInt(timer / 60);
                int seconds = Mathf.FloorToInt(timer % 60);
                timerText.text = $"{minutes:00}:{seconds:00}";
            }
        }

        //if (GameManager.gameState == "StageClear")
        //{
            
        //    // Rigidbody 고정 (움직임 방지)
        //    if (rb != null)
        //    {
        //        rb.velocity = Vector2.zero;
        //        rb.isKinematic = true; // 물리 영향 중지
        //    }

        //    //  애니메이션을 DeadAni로 고정
        //    if (animator != null)
        //    {
        //        animator.Play(DeadAni);
        //    }

            

        //    //  타이머 중지
        //    timerRunning = false;
        //}

    }


    IEnumerator StartTimer()
    {
        yield return new WaitForSeconds(1f); // Dead 애니메이션 살짝 보여주기용 대기 (선택)
        timer = timerDuration;
        timerRunning = true;
    }
}
