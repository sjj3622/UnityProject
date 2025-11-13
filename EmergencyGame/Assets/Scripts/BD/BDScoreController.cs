using UnityEngine;
using UnityEngine.UI;

public class BDScoreController : MonoBehaviour
{
    [Header("UI 연결")]
    public Text scoreText;   // UI 텍스트 (Score 표시용)

    [Header("점수 설정")]
    public int score = 0;            // 현재 점수
    public int goalScore = 100;      // 목표 점수 (Panel 표시 기준)

    void Start()
    {

        // 시작 시 점수 표시
        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"AddScore 호출: {amount}, 총점: {score}");
        UpdateScoreText();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
        else
            Debug.LogWarning("scoreText가 Inspector에 연결되지 않았습니다!");
    }

    void Update()
    {
        
    }
}
