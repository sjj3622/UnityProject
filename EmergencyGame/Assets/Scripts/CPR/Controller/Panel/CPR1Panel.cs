using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPR1Panel : MonoBehaviour
{
    public GameObject GamePanel;
    public GameObject ClaerPanel;


    void Start()
    {

        //시작시 Panel 비활성화
        if (GamePanel != null)
        {
            GamePanel.SetActive(false);
        }
        if (ClaerPanel != null)
        {
            ClaerPanel.SetActive(false);
        }
    }

    public void GameP()
    {
        
        if (GamePanel != null)
        {
            GamePanel.SetActive(true);
        }
    }

    public void ClaerP() 
    {
        if (ClaerPanel != null)
        {
            ClaerPanel.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
