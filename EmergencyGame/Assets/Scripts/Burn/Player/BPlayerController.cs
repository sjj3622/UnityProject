using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BPlayerController : MonoBehaviour
{
    BurnCanvas burnCanvas;
    ItemDropController itemdropController;
    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    bool isGame = false;
    bool isStopped = false;

    Vector2 moveDir;
    public Vector2 lastDir = Vector2.down;

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
        isStopped = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        burnCanvas = FindAnyObjectByType<BurnCanvas>();
        itemdropController = FindAnyObjectByType<ItemDropController>();


        // 초기 위치 Y값 조정
        Vector3 pos = transform.position;
        pos.y += groundOffsetY;
        transform.position = pos;

        // 초기 애니메이션
        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);

        // 바닥에 떨어지도록 중력 세팅 (강하게)
        rb.gravityScale = 2f;
    }

    private void Update()
    {


        // 플레이어 이동
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (BurngpManager.gameState == "Rescuer" || BurngpManager.gameState == "RescuerClear")
        {
            isStopped = false;
            v = 0;
        }
        if(BurngpManager.gameState == "RescuerGame" && !isGame)
        {
            isStopped = false;
            //isGame = true;
            v = 0;
            speed = itemdropController.sharedSpeed;
            
        }

        if (BurngpManager.gameState == "FireFighter")
        {
            Destroy(gameObject);

            //rb.gravityScale = 0f; // BStart 상태일 때 중력 제거
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
    private void LateUpdate()
    {
        ClampToCamera();
    }

    void ClampToCamera()
    {
        if (Camera.main == null) return;

        Vector3 pos = transform.position;

        // 카메라 좌표 변환
        Vector3 min = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 max = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        // 플레이어 스프라이트 반영
        float halfWidth = GetComponent<SpriteRenderer>().bounds.extents.x;
        float halfHeight = GetComponent<SpriteRenderer>().bounds.extents.y;

        pos.x = Mathf.Clamp(pos.x, min.x + halfWidth, max.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, min.y + halfHeight, max.y - halfHeight);

        transform.position = pos;
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Ground") && isStopped)
    //    {
    //        Debug.Log("플레이어 바닥 착지");
           
    //        if (BurngpManager.gameState == null)
    //        {
    //            burnCanvas.selectPanel.SetActive(true);
    //        }
    //        rb.gravityScale = 1f;        // 바닥에서는 중력 낮추기
    //        rb.velocity = Vector2.zero;  // 순간 속도 초기화
    //    }
    //}



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
