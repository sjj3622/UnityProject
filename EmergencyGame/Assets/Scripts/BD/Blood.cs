using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{

    public int scoreValue = 10; // 이 피가 줄 점수
    private BDScoreController scoreController;

    void Start()
    {
        // 씬에서 BDScoreController 찾기
        scoreController = FindAnyObjectByType<BDScoreController>();
    }

    public void ScorePlus()
    {
        Debug.Log("클릭시 메서드 시작");
        // 마우스로 클릭 시 점수 추가
        if (scoreController != null)
        {
            scoreController.AddScore(scoreValue);
        }

        // 클릭된 피 제거
        Destroy(gameObject);
    }

    void Update()
    {
        
    }
}
