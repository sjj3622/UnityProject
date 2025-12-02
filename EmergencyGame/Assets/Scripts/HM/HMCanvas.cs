using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HMCanvas : MonoBehaviour
{
    HMPlayerController hmPlayerController;
    HMgpManager hmgpManager;

    void Start()
    {
        hmPlayerController = FindAnyObjectByType<HMPlayerController>();
        hmgpManager = FindAnyObjectByType<HMgpManager>();
    }

    public void AgainClick()
    {
        //// 현재 씬에서 Gate 찾기
        //GameObject gate = GameObject.FindWithTag("Gate");
        //if (gate != null)
        //{
        //    // StartGate 갱신
        //    hmgpManager.StartGate = gate.transform;
        //    Debug.Log(gate.transform);
        //    // 플레이어 좌표 이동
        //    hmgpManager.player.position = hmgpManager.StartGate.position;
        //    Debug.Log(hmgpManager.player.position);
        //}
        //else
        //{
        //    Debug.LogWarning("현재 씬에는 StartGate 가 없습니다!");
        //}

        
        StartCoroutine(LoadHM());
    }

    public void ExitClick()
    {
        HMgpManager.gameState = null;
        SceneManager.LoadScene("Title");
    }

    IEnumerator LoadHM()
    {
        HMgpManager.gameState = null;
        hmgpManager.isClearing = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("HM", LoadSceneMode.Single);
    }

}
