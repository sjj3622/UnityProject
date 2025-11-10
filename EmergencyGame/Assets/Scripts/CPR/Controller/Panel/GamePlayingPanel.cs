using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayingPanel : MonoBehaviour
{
    // Inspector에서 직접 연결 가능하게 GameObject 배열로 선언
    public GameObject[] stages;

    // 현재 선택된 스테이지 인덱스
    private int currentIndex = 0;

    void Start()
    {
        // 시작 시 첫 번째 스테이지만 활성화
        UpdateStage();
    }

    public void NextClick()
    {
        currentIndex = (currentIndex + 1) % stages.Length;
        UpdateStage();
    }

    public void BackClick()
    {
        currentIndex = (currentIndex - 1 + stages.Length) % stages.Length;
        UpdateStage();
    }

    // 활성화 상태 업데이트
    void UpdateStage()
    {
        for (int i = 0; i < stages.Length; i++)
        {
            stages[i].SetActive(i == currentIndex);
        }
        
    }


}
