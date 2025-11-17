using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    bool isStopped = false;

    Vector2 moveDir;
    Vector2 lastDir = Vector2.down;

    [Header("Spawn Position Fix")]
    public float groundOffsetY = -0.1f; // 소환 시 Y값 미세 조정

    [Header("Animation Names")]
    public string stopUPAni = "BPlayerIdleBack";
    public string stopDOWNAni = "BPlayerIdle";
    public string stopLEFTAni = "BPlayerIdleLeft";
    public string stopRIGHTAni = "BPlayerIdleRight";

    public string runUPAni = "BPlayerBack";
    public string runDOWNAni = "BPlayerFront";
    public string runLEFTAni = "BPlayerLeft";
    public string runRIGHTAni = "BPlayerRight";

    string nowAni = "", oldAni = "";

    void Start()
    {
        //나중에 지울것
        BurngpManager.gameState = "BReady";
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // 바닥에 붙도록 y좌표 조정
        Vector3 pos = transform.position;
        pos.y += groundOffsetY;
        transform.position = pos;

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);
    }

    void Update()
    {
        if (isStopped) return;

        // --- 게임 상태별 중력 설정 ---
        if (BurngpManager.gameState == "BReady")
        {
            rb.gravityScale = 1f; // BReady 상태일 때 중력 적용
        }
        else if (BurngpManager.gameState == "BStart")
        {
            rb.gravityScale = 0f; // BStart 상태일 때 중력 제거
        }

        // --- 키보드 입력 받기 ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // BReady 상태에서는 상하 이동 제한
        if (BurngpManager.gameState == "BReady")
        {
            v = 0;
        }

        moveDir = new Vector2(h, v).normalized;

        // --- 이동 ---
        if (moveDir != Vector2.zero)
        {
            rb.velocity = moveDir * speed;
            SetMoveAnimation(moveDir);
        }
        else
        {
            rb.velocity = Vector2.zero;
            SetIdleAnimation();
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
        if (animator == null) return;

        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }
}
