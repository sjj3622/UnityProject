using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour
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

    public void ShowPanel() //버튼클릭시 Panel 활성화 함수
    {
        
        if (GamePanel != null)
        {
            GamePanel.SetActive(true);
        }
    }

    public void HidePanel() //버튼클릭시 Panel 비활성화 함수
    {
        if (GamePanel != null)
        {
            GamePanel.SetActive(false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
