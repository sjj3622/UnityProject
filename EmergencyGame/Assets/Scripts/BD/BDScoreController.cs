using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BDScoreController : MonoBehaviour
{
    public GameObject Panel; // 인스펙터에 연결
    public int score = 0;
    public Text scoreText;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"AddScore 호출: {amount}, 총점: {score}");

        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    void Start()
    {
        if (Panel != null)
            Panel.SetActive(false);
    }

    void Update()
    {
        if (Panel != null && score >= 100)
            Panel.SetActive(true);
    }
}
