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

    public void SoloBtnPanel() //��ưŬ���� Panel Ȱ��ȭ �Լ�
    {

        if (SoloPanel != null)
        {
            SoloPanel.SetActive(true);
            DuoPanel.SetActive(false);
        }
    }

    public void DuoBtnPanel() //��ưŬ���� Panel ��Ȱ��ȭ �Լ�
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

    public void ExitClick()   // ���� ���� ��ư
    {
        Application.Quit(); // ����Ƽ ���� ����

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;  //����Ƽ �������� ���۴��ߺκ�
#endif
    }


    public void CPR1Cilck()
    {
        SceneManager.LoadScene("CPR1");
        GameManager.gameState = "Stage1";
    }




    void Update()
    {
        
    }
}
