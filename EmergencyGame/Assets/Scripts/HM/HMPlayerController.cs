using UnityEngine;
using UnityEngine.SceneManagement;

public class HMPlayerController : MonoBehaviour
{
   

    Rigidbody2D rb;
    Animator animator;

    [Header("MOVE")]
    public float speed = 3.0f;
    public float jumpForce = 6.5f;
    bool isJump = false; // false = 바닥에 있음, true = 공중

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

    public string AkL = "HMIdleAL";
    public string AKR = "HMIdleAR";

    public string DL = "HMIdleDL";
    public string DR = "HMIdleDR";


    string nowAni = "", oldAni = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        transform.localScale = new Vector2(1f, 1f); // 기본 스케일

        nowAni = stopDOWNAni;
        oldAni = nowAni;
        animator.Play(nowAni);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            Destroy(gameObject);
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 inputDir = new Vector2(h, v);

        // gameState가 null일 때 (자유이동 + 중력0 + run/idle 애니)
        if (HMgpManager.gameState == null)
        {
            rb.gravityScale = 0f;

            if (inputDir.sqrMagnitude > 0.01f) // 움직일 때
            {
                rb.velocity = inputDir.normalized * speed;
                SetRunAnimation(inputDir.normalized);
                lastDir = inputDir.normalized; // 마지막 방향 저장
            }
            else // 멈췄을 때
            {
                rb.velocity = Vector2.zero;
                SetIdleAnimation();
            }

            return;
        }

        //  gameState가 HMStart일 때 (좌우이동 + 점프)
        if (HMgpManager.gameState == "HMStart")
        {
            transform.localScale = new Vector2(0.5f, 0.5f);
            rb.gravityScale = 3f;
            rb.velocity = new Vector2(h * speed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            {
                Jump();
            }
            if (isJump)
            {
                if (h > 0) nowAni = JumpR;
                else if (h < 0) nowAni = JumpL;
                ChangeAnimation();
            }

            if (!isJump)
            {
                if (h > 0) nowAni = runRIGHTAni;
                else if (h < 0) nowAni = runLEFTAni;
                else nowAni = lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni;
                ChangeAnimation();
            }

            if (h != 0) lastDir = new Vector2(h, 0);
        }
    }

    //  점프 + 방향 애니메이션
    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        isJump = true;

        nowAni = lastDir.x < 0 ? JumpL : JumpR;
        ChangeAnimation();
    }

    void SetRunAnimation(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y)) // 좌우 우선
            nowAni = dir.x > 0 ? runRIGHTAni : runLEFTAni;
        else // 상하 우선
            nowAni = dir.y > 0 ? runUPAni : runDOWNAni;

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
            isJump = false;
            nowAni = lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni;
            ChangeAnimation();
        }
    }
}
