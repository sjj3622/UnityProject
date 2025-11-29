using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BurnCanvas : MonoBehaviour
{
    public GameObject fireFighterPrefab;
    public GameObject selectPanel;

    BurngpManager burngpManager;

    void Start()
    {
        burngpManager = FindAnyObjectByType<BurngpManager>();
        selectPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }





    public void selectBtn1()
    {
        BurngpManager.gameState = "FireFighter";
        

        // 현재 BPlayer 찾기
        GameObject bPlayer = GameObject.Find("BPlayer(Clone)");  // 이름 정확히 맞춰야 함

        if (bPlayer != null)
        {
            // 위치 저장
            Vector3 spawnPos = bPlayer.transform.position;
            Quaternion spawnRot = bPlayer.transform.rotation;

            // 기존 플레이어 삭제
            Destroy(bPlayer);

            // FireFighter 프리팝 생성
            GameObject ff = Instantiate(fireFighterPrefab, spawnPos, spawnRot);
            ff.tag = "Player";
            DontDestroyOnLoad(ff);
        }

        selectPanel.SetActive(false);
    }
    public void selectBtn2()
    {
        BurngpManager.gameState = "Rescuer";
        Debug.Log(BurngpManager.gameState);
        selectPanel.SetActive(false);
    }


    public void AgainClick()
    {
        burngpManager.ClearPanel.SetActive(false);
        BurngpManager.gameState = null;
        SceneManager.LoadScene("Burn");
    }

    public void ExitClick()
    {
        BurngpManager.gameState = null;
        SceneManager.LoadScene("Title");
    }
}