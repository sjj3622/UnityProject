using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStartText : MonoBehaviour
{
    public TMP_Text tmpText;       // TextMeshPro 텍스트
    public string message = "Game Start"; // 표시할 메시지
    public float duration = 1.5f;  // 전체 애니메이션 시간

    private bool isone = false;
    void Start()
    {
        
        
    }
    void Update()
    {
        if (BDgpManager.gameState == "BDReady" && !isone)
        {
            isone = true;
            tmpText.text = message;
            tmpText.maxVisibleCharacters = 0; // 처음엔 글자 안보이게
            StartCoroutine(RevealText());
            
        }

    }
    IEnumerator RevealText()
    {
        
        int totalCharacters = tmpText.text.Length;
        float interval = duration / totalCharacters; // 글자 하나당 시간

        for (int i = 0; i <= totalCharacters; i++)
        {
            tmpText.maxVisibleCharacters = i; // 글자 수만큼 표시
            yield return new WaitForSeconds(interval);
            
        }
        BDgpManager.gameState = "BDStart";
        gameObject.SetActive(false);
    }
}
