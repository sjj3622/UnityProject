using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BDRulePanel : MonoBehaviour
{
    public GameObject Timer;
    public GameObject Score;
    public GameObject Count;

    public GameObject Btn1;
    public GameObject Btn2;
    public GameObject Btn3;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(BDgpManager.gameState == "BDReady")
        {
            Timer.SetActive(false);
            Score.SetActive(false);
            Count.SetActive(false);
        }
    }
}
