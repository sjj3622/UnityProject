using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMpatientController : MonoBehaviour
{
    public GameObject BackGround;
    public GameObject Img1;   // 깜빡거리게 할 오브젝트

    private Animator bgAnimator;
    HMPlayerController hmPlayerController;

    void Start()
    {
        Img1.SetActive(false);
        bgAnimator = BackGround.GetComponent<Animator>(); // Animator 가져오기
        hmPlayerController = FindAnyObjectByType<HMPlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("TriggerEnter2D 호출됨! 충돌 대상: " + collision.name + ", 태그: " + collision.tag);

        if (collision.CompareTag("Player"))
        {
            Debug.Log("Patient와 충돌 확인됨!");

            // 🔥 애니메이션 중지
            if (bgAnimator != null)
            {
                bgAnimator.speed = 0f;  // 완전 정지
                // 또는 → bgAnimator.enabled = false;
            }

            StartCoroutine(BlinkAndLoadScene());
        }
    }

    IEnumerator BlinkAndLoadScene()
    {
        float blinkTime = 3f;
        float interval = 0.2f;
        float timer = 0f;

        while (timer < blinkTime)
        {
            Img1.SetActive(!Img1.activeSelf);
            yield return new WaitForSeconds(interval);
            timer += interval;
        }

        Img1.SetActive(true);
        HMgpManager.gameState = "HMStart";
        hmPlayerController.isStopped = true;

        SceneManager.LoadScene("HMGamePlaying");
    }
}

