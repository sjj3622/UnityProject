using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPRCanvasController : MonoBehaviour
{

    public GameObject Panel;


    void Start()
    {
        // Panel만 활성화
        Panel.SetActive(true);
    }

    private void Update()
    {
        Debug.Log(GameManager.gameState);
    }

    public void OnClosePanel()
    {
        Panel.SetActive(false);
        GameManager.gameState = "gameStart";
    }
}
