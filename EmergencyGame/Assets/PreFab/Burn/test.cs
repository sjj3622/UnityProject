using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
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

    public float offsetX = 1f;
    public float offsetY = 0.7f; // Y 좌표 이동용 오프셋

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
            Debug.LogWarning("Animator component missing on PatientController!");

        ambulanceController = FindAnyObjectByType<AmbulanceController>();
        burngpManager = FindAnyObjectByType<BurngpManager>();
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
        }
    }

    void Update()
    {
        if (isFollowing && playerController != null)
        {
            switch (BurngpManager.gameState)
            {
                case "RescuerGame":
                    UpdatePositionRescuer();
                    UpdateAnimationRescuer();
                    break;

                case "FireFighter":
                    UpdatePositionFireFighter();
                    UpdateAnimationFireFighter();
                    break;
            }
        }

        // 앰뷸런스로 이동
        if (BurngpManager.gameState == "RescuerClear" && ambulanceController != null && isarrive)
        {
            MoveToAmbulance();
        }
    }

    // ===============================
    // 위치 업데이트
    // ===============================

    void UpdatePositionRescuer()
    {
        float dir = playerController.lastDir.x >= 0 ? 0.7f : -0.7f;
        transform.position = new Vector3(
            playerController.transform.position.x + offsetX * dir,
            playerController.transform.position.y,
            transform.position.z
        );
    }

    void UpdatePositionFireFighter()
    {
        float xDir = playerController.lastDir.x >= 0 ? 0.7f : -0.7f;
        float yDir = playerController.lastDir.y >= 0 ? 0.7f : -0.7f;

        transform.position = new Vector3(
            playerController.transform.position.x + offsetX * xDir,
            playerController.transform.position.y + offsetY * yDir,
            transform.position.z
        );
    }

    // ===============================
    // 애니메이션 업데이트
    // ===============================

    void UpdateAnimationRescuer()
    {
        if (animator == null) return;

        float dir = playerController.lastDir.x;
        if (dir >= 0)
            animator.Play(patientR);
        else
            animator.Play(patientL);
    }

    void UpdateAnimationFireFighter()
    {
        if (animator == null) return;

        Vector2 dir = playerController.lastDir;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
        {
            // X 방향이 우세
            animator.Play(dir.x >= 0 ? patientR : patientL);
        }
        else
        {
            // Y 방향이 우세
            animator.Play(dir.y >= 0 ? patientB : patientF);
        }
    }

    // ===============================
    // 앰뷸런스로 이동
    // ===============================

    void MoveToAmbulance()
    {
        float endX = ambulanceController.EndGate.position.x;
        if (Mathf.Abs(transform.position.x - endX) < 0.1f)
        {
            Destroy(gameObject);
            ambulanceController.isEnding = true;
            ambulanceController.isMoving = false;
        }
        else
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                new Vector3(endX, transform.position.y, transform.position.z),
                Time.deltaTime * 3f
            );
        }
    }
}
