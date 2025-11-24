using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientController : MonoBehaviour
{
    [Header("Animation Names")]
    public string patientL = "patientL";
    public string patientR = "patientR";
    public string patientF = "patientF";
    public string patientB = "patientB";

    private Animator animator;

    private BPlayerController playerController;
    private AmbulanceController ambulanceController;
    private BurngpManager burngpManager;

    private bool isFollowing = false;

    public bool isarrive = false;

    public float offsetX = 1f; // 플레이어 앞쪽 X 거리

    void Start()
    {
        ambulanceController = FindAnyObjectByType<AmbulanceController>();
        burngpManager = FindAnyObjectByType<BurngpManager>();
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogWarning("Animator component missing on PatientController!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BPlayer(Clone)" && !isFollowing)
        {
            burngpManager.IconPanel.SetActive(true);
            playerController = collision.gameObject.GetComponent<BPlayerController>();
            if (playerController == null)
            {
                Debug.LogError("BPlayerController not found on Player!");
                return;
            }

            isFollowing = true;
            BurngpManager.gameState = "RescuerGame";
            Debug.Log(BurngpManager.gameState);
            UpdatePosition();
        }

        
    }
    void Update()
    {
        if (isFollowing && playerController != null)
        {
            UpdatePosition();

            // 애니메이션 재생
            if (animator != null)
            {
                float dir = playerController.lastDir.x;
                if (dir >= 0)
                    animator.Play(patientR);
                else
                    animator.Play(patientL);
            }
        }
        if (BurngpManager.gameState == "RescuerClear" && ambulanceController != null && isarrive)
        {
            // EndGate X 좌표와 환자 위치 비교
            float endX = ambulanceController.EndGate.position.x;
            if (Mathf.Abs(transform.position.x - endX) < 0.1f) // 도착 범위 0.1f
            {
                Destroy(gameObject);
                ambulanceController.isEnding = true;
                ambulanceController.isMoving = false;
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

    void UpdatePosition()
    {
        // 플레이어가 바라보는 방향 기준으로 오브젝트 위치 지정
        float dir = playerController.lastDir.x >= 0 ? 0.7f : -0.7f;
        transform.position = new Vector3(playerController.transform.position.x + offsetX * dir,
                                         playerController.transform.position.y,
                                         transform.position.z);
    }
}
