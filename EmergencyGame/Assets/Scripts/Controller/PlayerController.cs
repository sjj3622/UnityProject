using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    float axisH, axisV;
    public float speed = 3.0f;
    bool isStopped = false;



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
    public static string gameState = "game";

    Vector2 moveDir; // 이동 방향 저장용
    Vector2 lastDir = Vector2.down; // 마지막 이동 방향 (정지 시 어떤 방향을 바라볼지)

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);
    }

    void Update()
    {
        if (gameState != "game") return;
        if (isStopped) return;
        axisH = Input.GetAxisRaw("Horizontal");
        axisV = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(axisH, axisV).normalized;

        // --- 방향에 따른 애니메이션 전환 ---
        if (moveDir.magnitude > 0.01f)
        {
            // 이동 중
            if (Mathf.Abs(moveDir.x) > Mathf.Abs(moveDir.y))
            {
                // 좌우 이동이 더 큼
                if (moveDir.x > 0)
                    nowAni = runRIGHTAni;
                else
                    nowAni = runLEFTAni;
            }
            else
            {
                // 상하 이동이 더 큼
                if (moveDir.y > 0)
                    nowAni = runUPAni;
                else
                    nowAni = runDOWNAni;
            }

            lastDir = moveDir; // 마지막 방향 기억
        }
        else
        {
            // 멈춤 상태
            if (Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
            {
                if (lastDir.x > 0)
                    nowAni = stopRIGHTAni;
                else
                    nowAni = stopLEFTAni;
            }
            else
            {
                if (lastDir.y > 0)
                    nowAni = stopUPAni;
                else
                    nowAni = stopDOWNAni;
            }
        }

        // --- 애니메이션 변경 ---
        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }

    private void FixedUpdate()
    {
        if (gameState != "game") return;

        rb.velocity = moveDir * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Patient"))
        {
            animator.Play(stopUPAni);
            gameState = "gamestart";
            Debug.Log(gameState);
            Gamestop();

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


        }
    }

    void Gamestop()
    {
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; // 물리효과 중단 (중력, 힘 등 무시)
        
    }
}
