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
        Debug.Log("totalTimer 값: " + timeValue);
        StarScroe();

    }

    public void StarScroe()
    {
        Debug.Log("별 스코어");
        float timeValue = timerController.totalTimer; // totalTimer 값 가져오기
        Debug.Log("totalTimer 값: " + timeValue);

        if (timeValue <= 180 && timeValue > 144)
        {
            Debug.Log("별4");
            nowAni = Star4;
        }
        else if (timeValue <= 144 && timeValue > 108)
        {
            Debug.Log("별3");
            nowAni = Star3;
        }
        else if (timeValue <= 108 && timeValue > 72)
        {
            Debug.Log("별2");
            nowAni = Star2;
        }
        else if (timeValue <= 72 && timeValue > 36)
        {
            Debug.Log("별1");
            nowAni = Star1;
        }
        else
        {
            Debug.Log("별0");
            nowAni = Star0;
        }

        ChangeAnimation();
    }

    void ChangeAnimation()
    {
        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }
}
