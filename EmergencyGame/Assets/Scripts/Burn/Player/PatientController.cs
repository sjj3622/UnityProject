using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientController : MonoBehaviour
{
    [Header("Animation Names")]
    public string patientL = "patientL";
    public string patientR = "patientR";

    private Animator animator;

    private BPlayerController playerController;

    private bool isFollowing = false;
    public float offsetX = 1f; // 플레이어 앞쪽 X 거리

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component missing on PatientController!");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "BPlayer" && !isFollowing)
        {
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
