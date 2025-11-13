using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedHoodController : MonoBehaviour
{
    private CameraManager camManager;

    private Animator animator;
    private Vector3 targetPosition;
    private bool isRunning = false;
    private bool isJumping = false;

    [Header("Settings")]
    public float minX;  // 시작 X 위치
    public float maxX = 8f;    // 목표 X 위치 범위
    public float minY;
    public float maxY = 6f;

    public float moveSpeed = 3f; // 이동 속도
    public float jumpHeight = 1.0f; // 점프 높이
    public float jumpDuration = 0.5f; // 점프 시간


    [Header("UI")]
    public Transform textObject; //여기에 Text 자식 오브젝트를 드래그해 놓기


    private Vector3 originalTextScale; // 원래 텍스트 스케일 저장용 변수




    void Start()
    {
        camManager = FindObjectOfType<CameraManager>();

        animator = GetComponent<Animator>();




        minX = transform.position.x;
        minY = transform.position.y;

        animator = GetComponent<Animator>();
        transform.position = new Vector3(minX, minY, transform.position.z);

        // 텍스트 원래 스케일 저장
        if (textObject != null)
            originalTextScale = textObject.localScale;

        // 랜덤 목표 위치 설정
        float randomX = Random.Range(-4.0f, maxX);
        float randomY = Random.Range(-6.0f, minY);

        targetPosition = new Vector3(randomX, randomY, transform.position.z);

        // 처음에는 Idle 재생
        animator.Play("RedHood");

        // 1초 후 Run 시작
        Invoke("StartRunning", 1f);
    }

    void Update()
    {
        if (isRunning)
        {
            MoveTowardsTarget();
        }
        // 여기서 텍스트 반전 방지 처리
        FixTextFlip();

        if(BDgpManager.gameState == "BDClear")
        {
            animator.Play("RedHood");
            // 자신의 Collider2D 비활성화
            Collider2D col = GetComponent<Collider2D>();
            if (col != null && col.enabled)
            {
                col.enabled = false;
            }

            // 이동 중이었다면 멈춤
            isRunning = false;
        }

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isJumping)
        {
            Debug.Log("충돌");
            camManager.OnPlayerPatientCollision();

            isRunning = false;
            //GetComponent<Collider2D>().enabled = false; // 충돌 비활성화
            StartCoroutine(JumpAndDie());
            Textupdate();

        }
    }




    void StartRunning()
    {
        animator.Play("RedHoodRun");
        isRunning = true;
    }

    void MoveTowardsTarget()
    {
        // 목표 위치까지 이동
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // 목표 위치 도달 시
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f && !isJumping)
        {
            // 방향 반전
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;

            // 새로운 목표 위치 설정 (현재 위치 기준 반대 방향으로)
            SetNewTarget();

            Debug.Log($"방향 반전됨, 새로운 타겟: {targetPosition}");
        }




        void SetNewTarget()
        {
            // -4.0 ~ 8.0 범위 내에서 랜덤 X값 선택
            float randomX = Random.Range(-4.0f, 8.0f);
            float randomY = Random.Range(-6.0f, 6.0f);

            // 목표 위치 설정 (Y, Z는 현재 위치 유지)
            targetPosition = new Vector3(randomX, randomY, transform.position.z);

            // 바라보는 방향을 랜덤 목표에 맞춰 반전
            if (randomX < transform.position.x)
            {
                // 왼쪽으로 가야 하므로 X축 반전
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
            {
                // 오른쪽으로 가야 하므로 X축 반전
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }

            Debug.Log($"새 목표 설정: {targetPosition}");
        }

    }




    IEnumerator JumpAndDie()
    {
        isJumping = true;
        animator.Play("RedHoodJump");

        Vector3 startPos = transform.position;
        Vector3 apex = new Vector3(transform.position.x, transform.position.y + jumpHeight, transform.position.z);
        float elapsed = 0f;

        // 점프 애니메이션 이동
        while (elapsed < jumpDuration)
        {
            transform.position = Vector3.Lerp(startPos, apex, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = apex;

        // 다시 땅으로
        elapsed = 0f;
        while (elapsed < jumpDuration)
        {
            transform.position = Vector3.Lerp(apex, startPos, elapsed / jumpDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = startPos;

        // Dead 애니메이션 재생
        animator.Play("RedHoodDead");
    }

    //텍스트 반전 방지 함수
    void FixTextFlip()
    {
        if (textObject != null)
        {
            Vector3 scale = originalTextScale;

            // 부모가 반전되었으면 자식 스케일에 -1을 곱해서 보정
            scale.x = Mathf.Sign(transform.localScale.x) * originalTextScale.x;
            scale.y = originalTextScale.y;
            scale.z = originalTextScale.z;

            textObject.localScale = scale;
        }
    }

    void Textupdate()
    {
        if (textObject != null)
        {
            TMPro.TextMeshPro tmpText = textObject.GetComponent<TMPro.TextMeshPro>();
            if (tmpText != null)
            {
                tmpText.text = "피가 많이 나요...";
            }

        }
    }



}