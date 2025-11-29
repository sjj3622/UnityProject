using System.Collections;
using UnityEngine;
using UnityEngine.UI; // UI Text 사용 위해 추가

public class patientController : MonoBehaviour
{
    public Transform tr;
    Rigidbody2D rb;
    Animator animator;
    
    GameManager gameManager;
    CPR1Panel cpr1Panel;

    [Header("Animation Names")]
    public string RunAni = "SantaIdle";
    public string DeadAni = "SantaDead";
    public string StataAni = "Stata";

    // string nowAni = "", oldAni = "";

    [Header("Move Settings")]
    public float speed = 2f; // 이동 속도
    public float targetX = -3f; // 도착 지점 X좌표

    bool isDead = false;
    public bool isCarried = false; // Medic이 태웠는지 여부






    void Start()
    {

        gameManager = FindAnyObjectByType<GameManager>();
        cpr1Panel = FindAnyObjectByType<CPR1Panel>();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        tr = transform;

        animator.Play(RunAni);

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
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 예: Player와 Patient가 충돌했을 때
        if (other.CompareTag("Patient") && this.CompareTag("Player"))
        {
           
            if (TimerController.instance != null)
                TimerController.instance.StartTimerDirectly();


            if (TimerController.instance.timer <= 0 && cpr1Panel.GamePanel.activeSelf)
            {
                Debug.Log("timer.totalTimer :" + TimerController.instance.timer);
                Debug.Log("panel.GamePanel.activeSelf" + cpr1Panel.GamePanel.activeSelf);

                gameManager.StartCoroutine(gameManager.GameOver());
            }
        }

        // 반대 경우도 처리
        if (other.CompareTag("Player") && this.CompareTag("Patient"))
        {
            if (TimerController.instance != null)
                TimerController.instance.StartTimerDirectly();

            if (TimerController.instance.timer <= 0 && cpr1Panel.GamePanel.activeSelf)
            {
                Debug.Log("timer.totalTimer :" + TimerController.instance.timer);
                Debug.Log("panel.GamePanel.activeSelf" + cpr1Panel.GamePanel.activeSelf);

                gameManager.StartCoroutine(gameManager.GameOver());
            }
        }
    }


}
