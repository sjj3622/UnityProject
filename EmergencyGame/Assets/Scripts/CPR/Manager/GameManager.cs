using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    
    public static string gameState;
    public GateController gate;
    public Transform patient;

    [Header("GamePanel")]
    [SerializeField] CPR1Panel panel;
    [SerializeField] Text messageText;

    [Header("OptionBtn")]
    [SerializeField] Button[] optionButtons;

    private string[][] optionTexts = new string[][]
    {
        new string[]
        {
            "구경만 한다.",
            "그냥 지나친다.",
            "쓰러진 사람의 상태를 확인한다.",
            "가만히 있는다."
        },
        new string[]
        {
            "119에 신고한다.",
            "다른사람 해주겠지",
            "무섭다 도망가자",
            "..."
        }
    };

    private string[] correctAnswer = { "쓰러진 사람의 상태를 확인한다.", "119에 신고한다." };
    private string[] WrongAnswer1 = { "구경만 한다.", "다른사람 해주겠지" };
    private string[] WrongAnswer2 = { "그냥 지나친다.", "무섭다 도망가자" };
    private string[] WrongAnswer3 = { "가만히 있는다.", "..." };

    private bool isQuestionShown = false;
    private int currentStep = 0; //  현재 질문 단계 (0=첫번째, 1=두번째)



    void Start()
    {

        Debug.Log("게임 스테이지 :" + gameState);
        //if (gameState == null) return;
        if (panel == null)
            panel = FindObjectOfType<CPR1Panel>();

        messageText.gameObject.SetActive(false);

        foreach (Button btn in optionButtons)
            btn.gameObject.SetActive(false);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].onClick.AddListener(() => StartCoroutine(OnOptionSelected(index)));
        }
        
        if (gameState == "StageClear")
        {
            Debug.Log("구급대원 출동");
            gate.SpawnMedic(patient);
        }

    }



    void Update()
    {
        

        if (GameObject.FindGameObjectWithTag("Patient") == null)
        {
            Debug.Log("타켓확인" + GameObject.FindGameObjectWithTag("Patient"));
            panel.ClaerPanel.SetActive(true);
        }


        //if (gameState != "gamestart") return;

        if (panel != null && panel.GamePanel.activeSelf && !isQuestionShown)
        {
            isQuestionShown = true;
            ShowQuestion(currentStep);
        }


        
    }

    void ShowQuestion(int step)
    {
        messageText.gameObject.SetActive(true);

        if (step == 0)
        {
            messageText.text = "사람이 쓰러졌다. 당신은 어떻게 할 것인가요?";
        }
        else if (step == 1)
        {
            messageText.text = "상태를 확인해보니 심장이 뛰지 않아! 다음에는 뭘 해야 하지?";
        }

        Shuffle(optionTexts[step]);
        for (int i = 0; i < optionButtons.Length; i++)
        {
            optionButtons[i].gameObject.SetActive(true);
            optionButtons[i].GetComponentInChildren<Text>().text = optionTexts[step][i];
        }
    }

    void SetButtonsInteractable(bool interactable)
    {
        foreach (Button btn in optionButtons)
        {
            btn.interactable = interactable;
        }
    }

    IEnumerator OnOptionSelected(int index)
    {

        //  버튼 클릭 금지
        SetButtonsInteractable(true);


        string selected = optionButtons[index].GetComponentInChildren<Text>().text;

        //  단계별 정답 체크
        if (selected == correctAnswer[currentStep])
        {

            Debug.Log($"정답! 단계 {currentStep + 1} : {selected}");
            
            currentStep++; // 다음 단계로 이동

            if (currentStep < optionTexts.Length)
            {
                ShowQuestion(currentStep); // 다음 질문 표시
                SetButtonsInteractable(true);
                
            }
            else
            {
                // 마지막 질문까지 완료
                messageText.text = "다음은 CPR!";
                SetButtonsInteractable(true);
                yield return new WaitForSeconds(3f);  // 3초 대기
                gameState = "StageClear";

                if (gameState == "StageClear")
                {
                    //panel.GamePanel.SetActive(false);
                    isQuestionShown = false;

                    // 다음 씬으로 이동
                    SceneStateManager.instance.SaveState(GameObject.Find("Patient"));
                    SceneStateManager.instance.SaveState(GameObject.Find("Timer"));
                    SceneStateManager.instance.SaveState(Camera.main.gameObject);
                    SceneManager.LoadScene("GamePlaying");
                    gameState = "StageRule";
                }
            }
        }
        else
        {
            Debug.Log("오답시");
            if (selected == WrongAnswer1[currentStep])
            {
                StartCoroutine(ShowWrongAnswerMessage("잠깐 구경만 해볼까?", "내가 할수 있는게 아니야", "아니야 다시 생각해보자"));
            }
            else if (selected == WrongAnswer2[currentStep])
            {
                StartCoroutine(ShowWrongAnswerMessage("그냥 지나가자...", "난 할 수 없어..", "아니야 다시 생각해보자"));
            }
            else if (selected == WrongAnswer3[currentStep])
            {
                StartCoroutine(ShowWrongAnswerMessage("누군가가 하겠지?", "...", "아니야 다시 생각해보자"));
            }
        }
    }





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

    IEnumerator ShowWrongAnswerMessage(string firstMessage, string secondMessage, string againMessage)
    {
        if (currentStep == 0)
        {
            messageText.text = firstMessage;
            SetButtonsInteractable(false);
            yield return new WaitForSeconds(1f);
            messageText.text = againMessage;
            yield return new WaitForSeconds(1f);
            messageText.text = "사람이 쓰러졌다. 당신은 어떻게 할 것인가요?";
            SetButtonsInteractable(true);
        }
        else if (currentStep == 1)
        {
            messageText.text = secondMessage;
            SetButtonsInteractable(false);
            yield return new WaitForSeconds(1f);
            messageText.text = againMessage;
            yield return new WaitForSeconds(1f);
            messageText.text = "상태를 확인해보니 심장이 뛰지 않아! 다음에는 뭘 해야 하지?";
            SetButtonsInteractable(true);
        }
    }



}
