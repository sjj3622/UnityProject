using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDRulePanel : MonoBehaviour
{
    public GameObject Timer;
    public GameObject Score;
    public GameObject Count;
    public GameObject StartText;

   

    public GameObject[] stages;


    private int currentIndex = 0;

    void Start()
    {
        UpdateStage();
    }

    // Update is called once per frame
    void Update()
    {
        if(BDgpManager.gameState == "BDReady")
        {
            Timer.SetActive(false);
            Score.SetActive(false);
            Count.SetActive(false);
            StartText.SetActive(false);
        }
    }

    public void Btn1XClick()
    {
        gameObject.SetActive(false);
        Timer.SetActive(true);
        Score.SetActive(true);
        Count.SetActive(true);
        StartText.SetActive(true);
        
    }

    public void Btn2RightClick()
    {
        currentIndex = (currentIndex + 1) % stages.Length;
        UpdateStage();
    }

    public void Btn3LeftClick()
    {
        currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
        UpdateStage();
    }

    void UpdateStage()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == currentIndex);
        }

    }




}
