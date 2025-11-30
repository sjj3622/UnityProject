using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMpatientController : MonoBehaviour
{
    public GameObject BackGround;
    public GameObject Img1;   // 깜빡거리게 할 오브젝트
    public GameObject Img2;
    public GameObject HMPlayer;
    public GameObject ClearPanel;
    public GameObject GameOverPanel;

    private bool iscol = false;
    public bool isclear = false;

    private Animator bgAnimator;
    private HMPlayerController hmPlayerController;

    void Start()
    {
        ClearPanel.SetActive(false);
        GameOverPanel.SetActive(false);

        Img1.SetActive(false);
        Img2.SetActive(false);
        bgAnimator = BackGround.GetComponent<Animator>(); // Animator 가져오기
        hmPlayerController = FindAnyObjectByType<HMPlayerController>();
    }

    void Update()
    {
        if (HMgpManager.gameState == "HMClear" && !isclear)
        {
            Img2.SetActive(true);
            // 기존 연출의 반대
            StartCoroutine(ReverseAnimatePlayerSequence());

            isclear = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (HMgpManager.gameState == null)
        {
            Debug.Log("왜??");
            if (collision.CompareTag("Player"))
            {
                if (bgAnimator != null)
                    bgAnimator.speed = 0f;  // 애니메이션 정지

                if (!iscol)
                {
                    StartCoroutine(BlinkAndThenAnimatePlayer());
                    iscol = true;
                }
            }
        }
    }

    IEnumerator BlinkAndThenAnimatePlayer()
    {
        // 1. Img1 깜빡이기
        float blinkTime = 3f;
        float interval = 0.2f;
        float timer = 0f;

        while (timer < blinkTime)
        {
            Img1.SetActive(!Img1.activeSelf);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        // Img1 비활성화, Img2 활성화
        Img1.SetActive(false);
        Img2.SetActive(true);

        // 2. HMPlayer 이동 & 크기 코루틴
        yield return StartCoroutine(AnimatePlayerSequence());

        // 3. 상태 변경 및 씬 전환
        HMgpManager.gameState = "HMStart";
        SceneManager.LoadScene("HMGamePlaying");
    }

    IEnumerator AnimatePlayerSequence()
    {
        // 시작 위치 & 크기
        HMPlayer.transform.position = new Vector3(-4f, 0f, 0f);
        HMPlayer.transform.localScale = new Vector3(1f, 1f, 1f);

        // 이동할 위치와 최종 크기
        Vector3[] positions = new Vector3[]
        {
        new Vector3(0f, 0f, 0f),    // 중간
        new Vector3(0f, -7f, 0f)    // 끝
        };

        Vector3[] scales = new Vector3[]
        {
        new Vector3(0.5f, 0.5f, 0.5f), // 중간 크기 (점점 작아짐)
        new Vector3(0.1f, 0.1f, 0.1f)  // 끝 크기
        };

        float duration = 1f; // 각 구간 이동 시간

        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 startPos = HMPlayer.transform.position;
            Vector3 targetPos = positions[i];
            Vector3 startScale = HMPlayer.transform.localScale;
            Vector3 targetScale = scales[i];

            float t = 0f;
            while (t < duration)
            {
                t += Time.deltaTime;
                float progress = t / duration;

                HMPlayer.transform.position = Vector3.Lerp(startPos, targetPos, progress);
                HMPlayer.transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

                yield return null;
            }

            // 정확히 위치와 크기 맞추기
            HMPlayer.transform.position = targetPos;
            HMPlayer.transform.localScale = targetScale;
        }
    }

    IEnumerator ReverseAnimatePlayerSequence()
    {

        // 1. 플레이어를 "사라졌던 마지막 위치 + 작은 크기"로 설정
        HMPlayer.transform.position = new Vector3(0f, -7f, 0f); // 원래 끝 위치
        HMPlayer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        // 이동 경로 (역방향)
        Vector3[] positions = new Vector3[]
        {
        new Vector3(0f, 0f, 0f),    // 중간 위치
        new Vector3(-4f, 0f, 0f)    // 원래 시작 위치
        };

        Vector3[] scales = new Vector3[]
        {
        new Vector3(0.5f, 0.5f, 0.5f), // 중간 크기
        new Vector3(1f, 1f, 1f)        // 원래 크기
        };

        float duration = 1f;

        // 2. 구간별 이동
        for (int i = 0; i < positions.Length; i++)
        {
            Vector3 startPos = HMPlayer.transform.position;
            Vector3 targetPos = positions[i];
            Vector3 startScale = HMPlayer.transform.localScale;
            Vector3 targetScale = scales[i];

            float t = 0f;

            while (t < duration)
            {
                t += Time.deltaTime;
                float progress = t / duration;

                HMPlayer.transform.position = Vector3.Lerp(startPos, targetPos, progress);
                HMPlayer.transform.localScale = Vector3.Lerp(startScale, targetScale, progress);

                yield return null;
            }

            // 정확히 위치와 크기 맞추기
            HMPlayer.transform.position = targetPos;
            HMPlayer.transform.localScale = targetScale;

            // 마지막 위치(처음 위치) 도달 시 Img2 비활성화
            if (i == positions.Length - 1)
            {
                Img2.SetActive(false);
                ClearPanel.SetActive(true);
            }
        }
    }



}


