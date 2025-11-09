using System.Collections;
using UnityEngine;

public class FTGameManager : MonoBehaviour
{
    [Header("References")]
    public GameObject player;      // Player 오브젝트
    public GameObject patient;     // Patient 오브젝트
    public GameObject panel;       // 활성화할 패널
    public GameObject textObject;  // Logo Text
    public GameObject patienttext; // Patient Text
    public GameObject OpBtn1Object; // 보기1
    public GameObject OpBtn2Object; // 보기2


    private PlayerControllerKey playerControllerKey;
    private Collider2D playerCollider;
    private Collider2D patientCollider;
    private LegacyText legacyText;

    private bool hasCollided = false;       // 충돌 처리 플래그
    private Coroutine textCoroutine = null; // 진행 중인 코루틴 저장

    void Start()
    {
        if (player != null)
        {
            playerControllerKey = player.GetComponent<PlayerControllerKey>();
            playerCollider = player.GetComponent<Collider2D>();
        }

        if (patient != null)
        {
            patientCollider = patient.GetComponent<Collider2D>();
        }

        if (textObject != null)
        {
            legacyText = textObject.GetComponent<LegacyText>();
        }

        if (panel != null)
        {
            panel.SetActive(false);
        }

        if (patienttext != null)
        {
            patienttext.SetActive(false);
        }



    }

    void Update()
    {

        if (playerCollider != null && patientCollider != null && !hasCollided)
        {
            // Player와 Patient가 겹치면
            if (playerCollider.IsTouching(patientCollider))
            {
                hasCollided = true; // 한 번만 처리되도록 플래그 설정

                if (playerControllerKey != null)
                    playerControllerKey.enabled = false;

                if (panel != null)
                    panel.SetActive(true);

                if (OpBtn1Object != null) OpBtn1Object.SetActive(false);
                if (OpBtn2Object != null) OpBtn2Object.SetActive(false);


                if (patienttext != null)
                    patienttext.SetActive(true);

                if (legacyText != null && legacyText.myText != null)
                {
                    legacyText.myText.text = "부딪혀서 넘어졌다!";

                    // 기존 코루틴이 실행 중이면 멈춤
                    if (textCoroutine != null)
                        StopCoroutine(textCoroutine);

                    textCoroutine = StartCoroutine(ChangeTextAfterDelay(3.0f, "다음으로 해야될것은?", OpBtn1Object, OpBtn2Object));
                    

                }
            }
        }
    }

    private IEnumerator ChangeTextAfterDelay(float delay, string newText, GameObject btn1, GameObject btn2)
    {
        yield return new WaitForSeconds(delay);

        if (legacyText != null && legacyText.myText != null)
        {
            legacyText.myText.text = newText;
        }

        // 2초 후 버튼 활성화
        if (btn1 != null) btn1.SetActive(true);
        if (btn2 != null) btn2.SetActive(true);

        textCoroutine = null;
    }

}
