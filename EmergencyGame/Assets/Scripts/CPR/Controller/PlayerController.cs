using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    bool isStopped = false;

    Vector2 moveDir;          // 이동 방향
    Vector2 lastDir = Vector2.down; // 마지막 이동 방향
    Vector2 targetPos;        // 마우스 클릭한 목적지
    bool isMoving = false;    // 이동 중 여부

    [Header("Animation Names")]
    public string stopUPAni = "PlayerIdleUP";
    public string stopDOWNAni = "PlayerIdleDOWN";
    public string stopLEFTAni = "PlayerIdleLEFT";
    public string stopRIGHTAni = "PlayerIdleRIGHT";

    public string runUPAni = "PlayerrunUP";
    public string runDOWNAni = "PlayerrunDOWN";
    public string runLEFTAni = "PlayerrunLEFT";
    public string runRIGHTAni = "PlayerrunRIGHT";

    string nowAni = "", oldAni = "";
    //public static string gameState = "game";

    Camera mainCam;

    TimerController timerController;
   

    void Start()
    {
        //GameManager.gameState = "game";
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;

        timerController = FindAnyObjectByType<TimerController>();
            

        

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);

        targetPos = transform.position;
        isStopped = false;

        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = true; // 충돌 다시 활성화
            Debug.Log("Collider 초기 활성화됨");
        }
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = false;
        }

    }

    void Update()
    {
        
        if (isStopped) return;
        // --- 마우스 클릭으로 목적지 설정 ---
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mouseWorld = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mouseWorld.z = 0;
            targetPos = mouseWorld;
            isMoving = true;
        }

        // --- 목적지로 이동 중이라면 방향 계산 ---
        if (isMoving)
        {
            Vector2 currentPos = transform.position;
            Vector2 dir = (targetPos - currentPos);
            float distance = dir.magnitude;

            if (distance < 0.05f)
            {
                // 목적지 도달
                rb.velocity = Vector2.zero;
                isMoving = false;
                SetIdleAnimation();
            }
            else
            {
                moveDir = dir.normalized;
                rb.velocity = moveDir * speed;
                SetMoveAnimation(moveDir);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void SetMoveAnimation(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            nowAni = dir.x > 0 ? runRIGHTAni : runLEFTAni;
        }
        else
        {
            nowAni = dir.y > 0 ? runUPAni : runDOWNAni;
        }

        lastDir = dir;
        ChangeAnimation();
    }

    void SetIdleAnimation()
    {
        if (Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
        {
            nowAni = lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni;
        }
        else
        {
            nowAni = lastDir.y > 0 ? stopUPAni : stopDOWNAni;
        }
        ChangeAnimation();
    }

    void ChangeAnimation()
    {
        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter2D 호출됨! 충돌 대상: " + collision.name + ", 태그: " + collision.tag);

        if (collision.CompareTag("Patient"))
        {
            Debug.Log("Patient와 충돌 확인됨!");
            animator.Play(stopUPAni);

            Debug.Log("123GameManager.gameState :" + GameManager.gameState);
            if (GameManager.gameState == null)
            {
                
               
                Debug.Log("타이머 시작");


                // 타이머 시작 호출
                TimerController timer = FindObjectOfType<TimerController>();
                if (timer != null)
                {
                    timer.StartTimerDirectly(); // <- 직접 호출 메서드 추가 필요
                }

                Gamestop(); // <- 타이머 시작한 뒤 충돌 비활성화

                CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
                if (camFollow != null)
                {
                    camFollow.enabled = true;
                    camFollow.SetFollowTarget(transform);
                }

                CPR1Panel panel = FindObjectOfType<CPR1Panel>();
                if (panel != null)
                {
                    panel.GameP();
                }

                GameManager.gameState = "gamestart";
            }

            if(GameManager.gameState == "StageClear")
            {
                Collider2D col = GetComponent<Collider2D>();
                if (col != null)
                {
                    Debug.Log("출동 방지");
                    col.enabled = false; // 충돌 꺼서 다른 오브젝트와 상호작용 안 함
                }
            }
        }
    }


    void Gamestop()
    {
        isStopped = true;

        // 충돌 비활성화
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false; // 충돌 꺼서 다른 오브젝트와 상호작용 안 함
        Debug.Log("캐릭터 충돌 비활성화");
        // Rigidbody 고정 (움직임 방지)
        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true; // 물리 영향 중지
        }
    }
}
