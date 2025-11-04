using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsPanel : MonoBehaviour
{
    public GameObject SoloPanel;
    public GameObject DuoPanel;
    void Start()
    {

        
        if (SoloPanel != null) 
        {
            SoloPanel.SetActive(false);
        }
    }

    public void SoloBtnPanel() //버튼클릭시 Panel 활성화 함수
    {

        if (SoloPanel != null)
        {
            SoloPanel.SetActive(true);
            DuoPanel.SetActive(false);
        }
    }

    public void DuoBtnPanel() //버튼클릭시 Panel 비활성화 함수
    {
        if (SoloPanel != null)
        {
            DuoPanel.SetActive(true);
            SoloPanel.SetActive(false);
        }
    }

    public void BackClick()
    {
        SceneManager.LoadScene("Title");

    }

    public void ExitClick()   // 게임 종료 버튼
    {
        Application.Quit(); // 유니티 게임 종료

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //유니티 에디터의 시작단추부분
#endif
    }


    public void CPR1Cilck()
    {
        SceneManager.LoadScene("CPR1");
    }




    void Update()
    {
        
    }
}
