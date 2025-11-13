using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class BDStarScore : MonoBehaviour
{

    Animator animator;

    [Header("Animation Names")]
    public string Star0 = "Star0";
    public string Star1 = "Star1";
    public string Star2 = "Star2";
    public string Star3 = "Star3";
    public string Star4 = "Star";

    string nowAni = "", oldAni = "";


    private BDTimerController bdtimerController;
 
    void Start()
    {
        bdtimerController = FindObjectOfType<BDTimerController>();
        animator = GetComponent<Animator>();

        if (bdtimerController == null)
            Debug.LogWarning("씬에서 TimerController를 찾을 수 없습니다!");

        gameObject.SetActive(false);


    }


    void Update()
    {

    }

    
    public void StarScroe()
    {
        Debug.Log("별 스코어");
        
        Debug.Log("totalTimer 값: " + bdtimerController.timer);

        if (bdtimerController.timer <= 180 && bdtimerController.timer > 144)
        {
            Debug.Log("별4");
            nowAni = Star4;
        }
        else if (bdtimerController.timer <= 144 && bdtimerController.timer > 108)
        {
            Debug.Log("별3");
            nowAni = Star3;
        }
        else if (bdtimerController.timer <= 108 && bdtimerController.timer > 72)
        {
            Debug.Log("별2");
            nowAni = Star2;
        }
        else if (bdtimerController.timer <= 72 && bdtimerController.timer > 36)
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
