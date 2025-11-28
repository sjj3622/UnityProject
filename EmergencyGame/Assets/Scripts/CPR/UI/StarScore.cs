using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;
using UnityEngine.UIElements;

public class StarScore : MonoBehaviour
{

    Animator animator;

    [Header("Animation Names")]
    public string Star0 = "Star0";
    public string Star1 = "Star1";
    public string Star2 = "Star2";
    public string Star3 = "Star3";
    public string Star4 = "Star4";

    //string nowAni = "", oldAni = "";


    public TimerController timerController;

    public int sceneIndex = 0;

    void Start()
    {
        if (timerController == null)
            timerController = FindObjectOfType<TimerController>();

        if (timerController == null)
            Debug.LogWarning("씬에서 TimerController를 찾을 수 없습니다!");


        animator = GetComponent<Animator>();



    }


    void Update()
    {
        float timeValue = timerController.totalTimer;
        Debug.Log("timeValue :" + timeValue);
        gameObject.SetActive(true);

        if (timerController != null)
            StarScroe();
        else
            Debug.LogWarning("TimerController가 연결되지 않았습니다!");



    }

    public void StarScroe()
    {
        float timeValue = timerController.totalTimer;

        
        int starCount = 0;

        if (timeValue <= 180 && timeValue > 144)
        {
            Debug.Log("별4");
            animator.Play(Star4);
            starCount = 4;
        }
        else if (timeValue <= 144 && timeValue > 108)
        {
            Debug.Log("별3");
            animator.Play(Star3);
            starCount = 3;
        }
        else if (timeValue <= 108 && timeValue > 72)
        {
            Debug.Log("별2");
            animator.Play(Star2);
            starCount = 2;
        }
        else if (timeValue <= 72 && timeValue > 36)
        {
            Debug.Log("별1");
            animator.Play(Star1);
            starCount = 1;
        }
        else
        {
            Debug.Log("별0");
            animator.Play(Star0);
            starCount = 0;
        }



        //  GameDataManager에 저장
        if (GameDataManager.Instance != null)
            GameDataManager.Instance.SetStar(sceneIndex, starCount);
        Debug.Log("starCount :" + starCount);

        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.SetStar(sceneIndex, starCount);
            StartCoroutine(GameDataManager.Instance.UploadGameData());
        }
    }




}