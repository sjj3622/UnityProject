using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static string gameState;

    [Header("GamePanel 관련")]
    [SerializeField] CPR1Panel panel;
    [SerializeField] Text messageText;

    [Header("보기 버튼들")]
    [SerializeField] Button[] optionButtons;

    // 보기 텍스트
    private string[] optionTexts = new string[]
    {
        "119에 신고한다",                 // 정답
        "그냥 지나친다",
        "주변 사람에게 도움을 요청한다",
        "심폐소생술을 시도한다"
    };

    private string correctAnswer = "119에 신고한다"; // 정답
    private bool isQuestionShown = false; // 한 번만 실행되도록 제어용 플래그

    void Start()
    {
        if (panel == null)
            panel = FindObjectOfType<CPR1Panel>();

        messageText.gameObject.SetActive(false);

        foreach (Button btn in optionButtons)
            btn.gameObject.SetActive(false);

        // 버튼 클릭 이벤트 등록
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i; // 클로저 문제 방지
            optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
        }
    }

    void Update()
    {
        // GamePanel이 켜지고, 아직 문제를 표시하지 않았다면 한 번만 실행
        if (panel != null && panel.GamePanel.activeSelf && !isQuestionShown)
        {
            isQuestionShown = true; // 다시 실행되지 않도록 설정

            messageText.gameObject.SetActive(true);
            messageText.text = "사람이 쓰러졌다. 당신은 어떻게 할 것인가요?";

            // 보기 순서 랜덤 섞기
            Shuffle(optionTexts);

            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = optionTexts[i];
            }
        }
    }

    // 버튼 클릭 시 실행
    void OnOptionSelected(int index)
    {
        string selected = optionButtons[index].GetComponentInChildren<Text>().text;

        if (selected == correctAnswer)
        {
            Debug.Log("정답입니다!");
            panel.GamePanel.SetActive(false); // GamePanel 비활성화
            isQuestionShown = false; // 다음 문제를 위해 초기화
        }
        else
        {
            Debug.Log("오답입니다. 다시 시도해보세요!");
        }
    }

    // Fisher-Yates 셔플
    void Shuffle<T>(T[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);
            T temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }
}
