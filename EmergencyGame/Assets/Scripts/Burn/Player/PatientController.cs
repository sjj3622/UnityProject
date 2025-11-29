using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientController : MonoBehaviour
{
    Rigidbody2D rb;

    [Header("MOVE")]
    public float speed = 5.0f;


    [Header("Animation Names")]
    public string patientL = "patientL";
    public string patientR = "patientR";
    public string patientF = "patientF";
    public string patientB = "patientB";

    private Animator animator;

    private BPlayerController playerController;
    private FFPlayerController ffPlayerController;
    private AmbulanceController ambulanceController;
    private BurngpManager burngpManager;

    private bool isFollowing = false;

    public bool isarrive = false;

    public float offsetX = 1f; // 플레이어 앞쪽 X 거리
    public float offsetY = 1f; // 플레이어 앞쪽 Y 거리


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        ambulanceController = FindAnyObjectByType<AmbulanceController>();
        burngpManager = FindAnyObjectByType<BurngpManager>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (animator == null)
            Debug.LogWarning("Animator component missing on PatientController!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BPlayer(Clone)" && !isFollowing && BurngpManager.gameState == "Rescuer")
        {
            burngpManager.IconPanel.SetActive(true);
            playerController = collision.gameObject.GetComponent<BPlayerController>();
            if (playerController == null)
            {
                Debug.LogError("BPlayerController not found on Player!");
                return;
            }
            rb.gravityScale = 1f;
            isFollowing = true;
            BurngpManager.gameState = "RescuerGame";
            Debug.Log(BurngpManager.gameState);
            BPlayerUpdatePosition();
        }

        if (collision.gameObject.name == "FireFighter(Clone)" && !isFollowing && (BurngpManager.gameState == "FireFighterClear" || BurngpManager.gameState == "FFStart"))
        {

            ffPlayerController = collision.gameObject.GetComponent<FFPlayerController>();
            if (ffPlayerController == null)
            {
                Debug.LogError("FFPlayerController not found on Player!");
                return;
            }

            rb.gravityScale = 0f;
            isFollowing = true;
            FFPlayerUpdatePosition();
        }

    }

    void Update()
    {
        

        if (BurngpManager.gameState == "FFStart")
        {
            isarrive = true;
        }

        if (isFollowing && playerController != null)
        {
            BPlayerUpdatePosition();


            // 애니메이션 재생
            if (animator != null)
            {
                if (playerController)
                {
                    float dir = playerController.lastDir.x;

                    if (dir >= 0)
                        animator.Play(patientR);
                    else
                        animator.Play(patientL);
                }

            }
        }

        if (isFollowing && ffPlayerController != null)
        {
            FFPlayerUpdatePosition();

            if (ffPlayerController)
            {
                float dirX = ffPlayerController.lastDir.x;
                float dirY = ffPlayerController.lastDir.y;

                // Y 방향이 먼저 우선 (위/아래 움직임)
                if (Mathf.Abs(dirY) > Mathf.Abs(dirX))
                {
                    if (dirY <= 0)
                        animator.Play(patientF);
                    else
                        animator.Play(patientB);
                }
                else
                {
                    if (dirX >= 0)
                        animator.Play(patientR);
                    else
                        animator.Play(patientL);
                }
            }


        }

        if ((BurngpManager.gameState == "RescuerClear" || BurngpManager.gameState == "FireFighterClear") && isarrive) //&& ambulanceController != null || BurngpManager.gameState == "FireFighterClear"
        {
            if (ambulanceController == null)
            {
                ambulanceController = FindAnyObjectByType<AmbulanceController>();
                if (ambulanceController == null)
                    return; // 아직도 없으면 다음 프레임까지 기다림
            }

            if (ambulanceController == null)
            {
                Debug.LogError("AmbulanceController is NULL!");
            }
            else if (ambulanceController.EndGate == null)
            {
                Debug.LogError("EndGate is NULL on AmbulanceController!");
            }
            // EndGate X 좌표와 환자 위치 비교
            float endX = ambulanceController.EndGate.position.x;
            if (Mathf.Abs(transform.position.x - endX) < 0.1f) // 도착 범위 0.1f
            {
                Destroy(gameObject);
                ambulanceController.isEnding = true;
                ambulanceController.isMoving = false;
                ambulanceController.isClear = true;
            }
            else
            {
                // EndGate로 이동
                transform.position = Vector3.MoveTowards(
                    transform.position,
                    new Vector3(endX, transform.position.y, transform.position.z),
                    Time.deltaTime * 3f // 속도
                );
            }
        }

    }

    void BPlayerUpdatePosition()
    {
        // 플레이어가 바라보는 방향 기준으로 오브젝트 위치 지정
        float dir = playerController.lastDir.x >= 0 ? 0.7f : -0.7f;
        transform.position = new Vector3(playerController.transform.position.x + offsetX * dir,
                                         playerController.transform.position.y,
                                         transform.position.z);
    }

    void FFPlayerUpdatePosition()
    {
        float dirX = ffPlayerController.lastDir.x;
        float dirY = ffPlayerController.lastDir.y;

        transform.position = new Vector3(
            ffPlayerController.transform.position.x + offsetX * dirX,
            ffPlayerController.transform.position.y + offsetY * dirY,
            transform.position.z
        );
    }

}
