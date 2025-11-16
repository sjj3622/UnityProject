using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class StarScore : MonoBehaviour
{

    Animator animator;

    [Header("Animation Names")]
    public string Star0 = "Star0";
    public string Star1 = "Star1";
    public string Star2 = "Star2";
    public string Star3 = "Star3";
    public string Star4 = "Star";

    string nowAni = "", oldAni = "";


    public TimerController timerController;

    public int sceneIndex =0;

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

        gameObject.SetActive(true);

        if (timerController != null)
            UpdateStarAnimation();
        else
            Debug.LogWarning("TimerController가 연결되지 않았습니다!");



    }

    void UpdateStarAnimation()
    {
        float timeValue = timerController.totalTimer;
        //Debug.Log("totalTimer 값: " + timeValue);
        StarScroe();

    }

    public void StarScroe()
    {
        float timeValue = timerController.totalTimer;

        int starCount = 0;

        if (timeValue <= 180 && timeValue > 144)
        {
            nowAni = Star4;
            starCount = 4;
        }
        else if (timeValue <= 144 && timeValue > 108)
        {
            nowAni = Star3;
            starCount = 3;
        }
        else if (timeValue <= 108 && timeValue > 72)
        {
            nowAni = Star2;
            starCount = 2;
        }
        else if (timeValue <= 72 && timeValue > 36)
        {
            nowAni = Star1;
            starCount = 1;
        }
        else
        {
            nowAni = Star0;
            starCount = 0;
        }

        ChangeAnimation();

        // 별점 저장
        GameDataManager.Instance.SetStar(sceneIndex, starCount);
    }


    void ChangeAnimation()
    {
        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }

    public void SetStarAnimation(int starCount)
    {
        switch (starCount)
        {
            case 4: animator.Play(Star4); break;
            case 3: animator.Play(Star3); break;
            case 2: animator.Play(Star2); break;
            case 1: animator.Play(Star1); break;
            default: animator.Play(Star0); break;
        }
    }
}