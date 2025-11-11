using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CPR1Panel : MonoBehaviour
{
    public GameObject GamePanel;
    public GameObject ClaerPanel;
    public GameObject TimerText;

    public SceneStateManager SM;

    void Start()
    {
        
        SM = SceneStateManager.instance;
        TimerText.SetActive(true);

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
    
    public void AgainClick()
    {
        GameManager.gameState = "game";
        TimerText.SetActive(false);
        SM.ClearSaved();
        SceneManager.LoadScene("CPR");
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Title");
    }



    void Update()
    {
        
    }
}
