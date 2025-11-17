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

    Vector2 moveDir;          // 이동 방향
    Vector2 lastDir = Vector2.down; // 마지막 이동 방향

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


        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);
    }

    void Update()
    {
        if (isStopped) return;

        // --- 키보드 입력 받기 ---
        float h = Input.GetAxisRaw("Horizontal"); // A, D 또는 ←, →
        float v = Input.GetAxisRaw("Vertical");   // W, S 또는 ↑, ↓

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
        if (animator == null) return; // animator가 없으면 그냥 리턴

        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }
}
