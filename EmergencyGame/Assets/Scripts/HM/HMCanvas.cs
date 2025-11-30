using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMCanvas : MonoBehaviour
{
    HMPlayerController hmPlayerController;
    void Start()
    {
        hmPlayerController = FindAnyObjectByType<HMPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AgainClick()
    {

        HMgpManager.gameState = null;


        // 현재 씬에서 플레이어 오브젝트 제거
        if (hmPlayerController != null)
        {
            Destroy(hmPlayerController.gameObject);
        }

        SceneManager.LoadScene("HM");
    }

    public void ExitClick()
    {
        HMgpManager.gameState = null;
        SceneManager.LoadScene("Title");
    }
}
