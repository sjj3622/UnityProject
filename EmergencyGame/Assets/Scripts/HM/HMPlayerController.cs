using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HMPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer sr;

    HMpatientController hmpatientController;

    [Header("MOVE")]
    public float speed = 3.0f;
    public float jumpForce = 6.5f;


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

    public string Clear = "HMIdleClear";

    [Header("Damage Settings")]
    public float knockbackDistance = 2f;
    public float invincibilityTime = 1f;


    public GameObject boomClone;

    // -------------------------
    // 상태 관리
    // -------------------------
    enum PlayerState { Idle, Run, Jump, Attack, Damage }
    PlayerState state = PlayerState.Idle;

    Vector2 lastDir = Vector2.down;
    string currentAni = "";

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        hmpatientController = FindAnyObjectByType<HMpatientController>();


        transform.localScale = new Vector2(1f, 1f);

        PlayAnimation(stopDOWNAni);
    }

    void Update()
    {
        

        if (SceneManager.GetActiveScene().name == "Title")
        {
            Destroy(gameObject);
            return;
        }

        //-----------------------------
        // 클리어 상태
        //-----------------------------
        if (HMgpManager.gameState == "HMClear")
        {
            rb.gravityScale = 0f;
            rb.velocity = Vector2.zero;

            if (hmpatientController.isclear)
            {
                animator.Play(Clear);
            }
            else
            {
                SetIdleAnimation();
            }

                return;
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector2 inputDir = new Vector2(h, v).normalized;

        //---------------------------------------------------
        // HMStart 이전 → 자유 이동(탑뷰)
        //---------------------------------------------------
        if (HMgpManager.gameState == null)
        {
            rb.gravityScale = 0f;
            rb.velocity = inputDir * speed;

            if (inputDir.sqrMagnitude > 0.01f)
            {
                lastDir = inputDir;
                state = PlayerState.Run;
                SetRunAnimation(inputDir);
            }
            else
            {
                state = PlayerState.Idle;
                SetIdleAnimation();
            }
            return;
        }

        //---------------------------------------------------
        // HMStart 이후 → 점프/넉백 있는 사이드뷰 이동
        //---------------------------------------------------
        if (HMgpManager.gameState == "HMStart")
        {
            rb.gravityScale = 3f;
            transform.localScale = new Vector2(0.5f, 0.5f);

            switch (state)
            {
                case PlayerState.Idle:
                case PlayerState.Run:
                    rb.velocity = new Vector2(h * speed, rb.velocity.y);

                    if (h != 0) lastDir = new Vector2(h, 0);

                    if (Input.GetKeyDown(KeyCode.Space))
                        Jump();

                    if (Input.GetKeyDown(KeyCode.Z))
                        StartAttack();

                    if (state != PlayerState.Jump && state != PlayerState.Attack && state != PlayerState.Damage)
                    {
                        if (Mathf.Abs(h) > 0.1f)
                        {
                            state = PlayerState.Run;
                            PlayAnimation(h > 0 ? runRIGHTAni : runLEFTAni);
                        }
                        else
                        {
                            state = PlayerState.Idle;
                            PlayAnimation(lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni);
                        }
                    }
                    break;

                case PlayerState.Jump:
                    rb.velocity = new Vector2(h * speed, rb.velocity.y);
                    if (h != 0)
                    {
                        lastDir = new Vector2(h, 0);
                        PlayAnimation(lastDir.x < 0 ? JumpL : JumpR);
                    }
                    break;

                case PlayerState.Attack:
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    break;

                case PlayerState.Damage:
                    // 넉백 중에는 velocity를 Update에서 막지 않음
                    break;
            }
        }
    }

    void Jump()
    {
        if (state == PlayerState.Jump || state == PlayerState.Attack || state == PlayerState.Damage) return;

        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        state = PlayerState.Jump;

        PlayAnimation(lastDir.x < 0 ? JumpL : JumpR);
    }

    void StartAttack()
    {
        if (state == PlayerState.Attack || state == PlayerState.Damage) return;

        state = PlayerState.Attack;
        PlayAnimation(lastDir.x < 0 ? AkL : AKR);

        // 플레이어 기준 앞쪽 위치 계산
        Vector3 attackPos = transform.position;
        float offsetX = 1f; // 앞쪽 X 거리
        float offsetY = 0f; // 필요하면 Y 오프셋 추가

        // 좌/우 방향에 따라 앞쪽 위치 조정
        attackPos += new Vector3(lastDir.x < 0 ? -offsetX : offsetX, offsetY, 0f);

        Instantiate(boomClone, attackPos, Quaternion.identity);

        Invoke(nameof(EndAttack), 0.5f);
    }

    void EndAttack()
    {
        if (state != PlayerState.Attack) return;

        state = PlayerState.Idle;
        PlayAnimation(lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni);
    }

    // -------------------------
    // 데미지 + 넉백 + 깜빡임
    // -------------------------
    public void TakeDamage(Transform enemyTransform)
    {
        if (state == PlayerState.Damage) return;

        state = PlayerState.Damage;

        // 넉백 방향 계산: Enemy 반대 방향
        Vector2 knockDir = (transform.position - enemyTransform.position).normalized;
        knockDir.y = 0; // 수평 넉백
        rb.velocity = Vector2.zero;
        rb.gravityScale = 1f; // 넉백이 적용되도록 gravity 활성화

        float knockbackStrength = 6f;
        rb.AddForce(knockDir * knockbackStrength, ForceMode2D.Impulse);

        // 데미지 애니메이션
        if (knockDir.x > 0)
            PlayAnimation(DR);
        else
            PlayAnimation(DL);

        StartCoroutine(InvincibilityCoroutine());
    }

    IEnumerator InvincibilityCoroutine()
    {
        float timer = 0f;
        while (timer < invincibilityTime)
        {
            sr.enabled = !sr.enabled;
            timer += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        sr.enabled = true;

        state = PlayerState.Idle;
        PlayAnimation(lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            if (state == PlayerState.Jump)
            {
                state = PlayerState.Idle;
                PlayAnimation(lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni);
            }
        }

        if (collision.gameObject.CompareTag("Enemy"))
            TakeDamage(collision.transform);
    }

    void SetRunAnimation(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            PlayAnimation(dir.x > 0 ? runRIGHTAni : runLEFTAni);
        else
            PlayAnimation(dir.y > 0 ? runUPAni : runDOWNAni);
    }

    void SetIdleAnimation()
    {
        if (Mathf.Abs(lastDir.x) > Mathf.Abs(lastDir.y))
            PlayAnimation(lastDir.x > 0 ? stopRIGHTAni : stopLEFTAni);
        else
            PlayAnimation(lastDir.y > 0 ? stopUPAni : stopDOWNAni);
    }

    void PlayAnimation(string aniName)
    {
        if (currentAni == aniName) return;
        currentAni = aniName;
        animator.Play(currentAni);
    }
}
