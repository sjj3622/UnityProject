using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BDCountController : MonoBehaviour
{
    public Text bleedCountText;
    public int bloodCount;

    void Update()
    {
        // "blood" 태그가 붙은 모든 오브젝트를 찾아 개수 갱신
        bloodCount = GameObject.FindGameObjectsWithTag("blood").Length;

        // UI 텍스트에 표시
        if (bleedCountText != null)
        {
            bleedCountText.text = "Blood Count: " + bloodCount;
        }
    }
}
