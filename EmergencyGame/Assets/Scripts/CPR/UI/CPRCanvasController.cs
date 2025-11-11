using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPRCanvasController : MonoBehaviour
{

    public GameObject Panel;
    public GameObject timeText;

    public TimerController timer;

    void Start()
    {
        // Panel만 활성화
        Panel.SetActive(true);
        timeText.SetActive(true);

        
        
        
    }

    private void Update()
    {
        Debug.Log(GameManager.gameState);
        //timer.TR();
        timer.TimerSave();
    }

    public void OnClosePanel()
    {
        Panel.SetActive(false);
        GameManager.gameState = "gameStart";
    }
}
