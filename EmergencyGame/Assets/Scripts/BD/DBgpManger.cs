using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBgpManger : MonoBehaviour
{
    GameObject Panel;

    private BleedController bleedController;
    private Mouse Mouse;
    private BDScoreController bdscoreController;


    void Start()
    {

        // 씬에 있는 BleedController 자동 찾기
        bleedController = FindAnyObjectByType<BleedController>();
        Mouse = FindAnyObjectByType<Mouse>();
        bdscoreController = FindAnyObjectByType<BDScoreController>();

        
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Mouse.RemoveClickedBleed();
        }

        if(bdscoreController.score >= 100)
        {
            
        }
    }



}
