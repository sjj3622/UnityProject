using UnityEngine;
using System.Collections;

public class AmbulanceController : MonoBehaviour
{
    public GameObject player;      // 씬에 있는 플레이어
    public float speed = 2f;       // 앰뷸런스 이동 속도
    public float targetX = -7f;    // 목표 X 위치
    public float returnX = -20f;   // 돌아갈 X 위치

    private bool isMovingToTarget = true;  // 목표 좌표로 이동 중
    private bool isReturning = false;      // 되돌아가는 중
    private Vector3 originalScale;         // 원래 스케일 저장

    private SpriteRenderer sr;             // 앰뷸런스 SpriteRenderer

    void Start()
    {
        originalScale = transform.localScale;
        sr = GetComponent<SpriteRenderer>();



        if (player == null)
        {
            player = GameObject.FindWithTag("Player"); // 플레이어 태그로 자동 참조
            if (player == null)
            {
                Debug.LogError("씬에 Player 오브젝트가 없습니다!");
            }
        }

        // 플레이어를 앰뷸런스 위치로 이동
        if (player != null)
        {
            player.transform.position = transform.position;
        }

        // 이동 중에는 앰뷸런스가 플레이어보다 위
        if (sr != null) sr.sortingOrder = 1;
    }

    void Update()
    {
        if (BurngpManager.gameState == null)
        {
            recall();
        }
        else if (BurngpManager.gameState == "RescuerClear")
        {
            Debug.Log("출발");

            // 출발 전에 위치 초기화 (한 번만)
            if (!isMovingToTarget && !isReturning)
            {
                GameObject ground = GameObject.FindWithTag("Ground");
                if (ground != null)
                {
                    float leftX = ground.GetComponent<Renderer>().bounds.min.x;
                    float yPos = transform.position.y; // 원래 y 위치 유지
                    transform.position = new Vector3(leftX, yPos, transform.position.z);

                    if (player != null)
                        player.transform.position = transform.position;
                }
                else
                {
                    Debug.LogError("씬에 Ground 오브젝트가 없습니다!");
                }

                // 상태 초기화
                isMovingToTarget = true;
                isReturning = false;

                // 스프라이트 정렬 초기화
                if (sr != null)
                    sr.sortingOrder = 1;

                // 스케일 초기화
                transform.localScale = originalScale;
            }

            // recall() 계속 호출
            recall();
        }
        else if (BurngpManager.gameState == "Rescuer")
        {
            if (player != null) player.SetActive(true);
            gameObject.SetActive(false);
        }
        else if (BurngpManager.gameState == "FireFighter")
        {
            if (player != null) player.SetActive(false);
            gameObject.SetActive(false);
        }
    }





    void recall()
    {
        float step = speed * Time.deltaTime;

        if (isMovingToTarget)
        {
            // 앰뷸런스 이동
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(targetX, transform.position.y, transform.position.z), step);

            // 목표 이동 중일 때만 플레이어도 이동
            if (player != null && !isReturning)
                player.transform.position = transform.position;

            // 목표 위치 도착
            if (Mathf.Approximately(transform.position.x, targetX))
            {
                isMovingToTarget = false;

                if (sr != null) sr.sortingOrder = -1;

                StartCoroutine(WaitAndReturn(1f));
            }
        }
        else if (isReturning)
        {
            // 돌아갈 때는 앰뷸런스만 이동
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(returnX, transform.position.y, transform.position.z), step);

            if (Mathf.Approximately(transform.position.x, returnX))
            {
                isReturning = false;
                gameObject.SetActive(false);
            }
        }
    }


    IEnumerator WaitAndReturn(float waitTime)
    {
        // 1초 대기
        yield return new WaitForSeconds(waitTime);

        // 앰뷸런스 반전
        FlipDirection();
        isReturning = true;

        // 돌아갈 때도 앰뷸런스가 플레이어보다 위
        if (sr != null) sr.sortingOrder = -1;
    }

    void FlipDirection()
    {
        transform.localScale = new Vector3(-originalScale.x, originalScale.y, originalScale.z);
    }
}