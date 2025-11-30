using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FFPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    bool isStopped = false;

    Vector2 moveDir;
    public Vector2 lastDir = Vector2.down;

    [Header("Spawn Position Fix")]
    public float groundOffsetY = -0.1f; // 소환 시 Y값 미세 조정

    [Header("Animation Names")]
    public string FFIdleF = "FFIdleF";
    public string FFIdleB = "FFIdleB";
    public string FFIdleL = "FFIdleL";
    public string FFIdleR = "FFIdleR";

    public string FFIdleFR = "FFIdleFR";
    public string FFIdleBR = "FFIdleBR";
    public string FFIdleLR = "FFIdleLR";
    public string FFIdleRR = "FFIdleRR";

    public string nowAni = "", oldAni = "";

    private bool isCollidingWithPatient = false;

    void Start()
    {
        isStopped = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();


        // 초기 위치 Y값 조정
        Vector3 pos = transform.position;
        pos.y += groundOffsetY;
        transform.position = pos;

        // 초기 애니메이션
        nowAni = FFIdleF;
        oldAni = nowAni;
        animator.Play(nowAni);

    }

    private void Update()
    {
        if (BurngpManager.gameState == null)
        {
            Destroy(gameObject);
        }

        if (BurngpManager.gameState == "Rescuer")
        {
            nowAni = FFIdleRR;
            ChangeAnimation();
            isCollidingWithPatient = true;

        }


        // 플레이어 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (BurngpManager.gameState == "FireFighter" || BurngpManager.gameState == "FireFighterClear")
        {
            isStopped = false;
            rb.gravityScale = 1f;
            v = 0;

        }
        if (BurngpManager.gameState == "FFStart")
        {
            if (SceneManager.GetActiveScene().name == "HouseFire")
            {
                rb.gravityScale = 0f;
            }
        }

        if (isStopped) return;  // 바닥 충돌 전 이동 금지


        moveDir = new Vector2(h, v).normalized;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Patient"))
        {
            if (isCollidingWithPatient)
            {
                nowAni = FFIdleF;
                ChangeAnimation();
                rb.velocity = Vector2.zero;         // 이동 멈춤
            }
        }
    }



    void SetMoveAnimation(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            nowAni = dir.x > 0 ? FFIdleRR : FFIdleLR;
        }
        else
        {
            nowAni = dir.y > 0 ? FFIdleBR : FFIdleFR;
        }

        lastDir = dir;
        ChangeAnimation();
    }

    void SetIdleAnimation()
    {
        if (Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
        {
            nowAni = lastDir.x > 0 ? FFIdleR : FFIdleL;
        }
        else
        {
            nowAni = lastDir.y > 0 ? FFIdleB : FFIdleF;
        }
        ChangeAnimation();
    }

    public void ChangeAnimation()
    {
        if (animator == null) return;

        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }
}