using UnityEngine;

public class HMPlayerController : MonoBehaviour
{
    static HMPlayerController instance;

    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    public bool isStopped = false;
    bool isJump = true;

    Vector2 moveDir;
    Vector2 lastDir = Vector2.down;

    [Header("Animation Names")]
    public string stopUPAni = "HMIdleB";
    public string stopDOWNAni = "HMIdleF";
    public string stopLEFTAni = "HMIdleL";
    public string stopRIGHTAni = "HMIdleR";

    public string runUPAni = "HMIdleBR";
    public string runDOWNAni = "HMIdleFR";
    public string runLEFTAni = "HMIdleLR";
    public string runRIGHTAni = "HMIdleRR";

    public string JumpL = "HMIdleJump";
    public string JumpR = "HMIdleJR";



    string nowAni = "", oldAni = "";

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        transform.localScale = new Vector2(1f, 1f); // 기본 스케일

        if (rb != null)
        {
            rb.gravityScale = 0f;
        }

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);
    }

    void Update()
    {
        if (animator == null || rb == null) return;

        

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        

        // 게임 시작 상태 처리
        if (HMgpManager.gameState == "HMStart")
        {

            transform.localScale = new Vector2(0.5f, 0.5f);
            rb.gravityScale = 3.0f;
            v = 0;
            if (!isJump)
            {
                isStopped = false;
                rb.gravityScale = 5.0f;
               
            }

        }
        if (isStopped) return;


        moveDir = new Vector2(h, v);

        if (moveDir.sqrMagnitude > 0.01f)
        {
            Vector2 dir = moveDir.normalized;
            rb.velocity = dir * speed;
            SetMoveAnimation(dir);
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
            nowAni = dir.x > 0 ? runRIGHTAni : runLEFTAni;
        else
            nowAni = dir.y > 0 ? runUPAni : runDOWNAni;

        lastDir = dir;
        ChangeAnimation();
    }

    void SetIdleAnimation()
    {
        if (Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
            nowAni = lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni;
        else
            nowAni = lastDir.y > 0 ? stopUPAni : stopDOWNAni;

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


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Debug.Log("바닥에 닿음 (물리 충돌)");
            isJump = false;
        }
        else
        {
            isJump = true;
        }
    }

}
