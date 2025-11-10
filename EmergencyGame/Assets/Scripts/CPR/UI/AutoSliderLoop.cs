using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AutoSliderLoop : MonoBehaviour
{
    public Slider targetSlider;
    public float duration = 2.0f;
    private float currentValue = 0f;

    void Start()
    {
        
    }


    private void Update()
    {
        if (GameManager.gameState != "gameStart") return;

        if (targetSlider == null)
            targetSlider = GetComponent<Slider>();

        targetSlider.minValue = 0f;
        targetSlider.maxValue = 1f;
        targetSlider.value = currentValue;

        StartCoroutine(MoveSliderRoutine());
    }


    IEnumerator MoveSliderRoutine()
    {
        while (true)
        {
            float timer = 0f;

            // 0 → 1로 이동
            while (timer < duration)
            {
                timer += Time.deltaTime;
                targetSlider.value = Mathf.Lerp(0f, 1f, timer / duration);
                yield return null;
            }

            // 끝까지 갔으면 즉시 0으로 리셋
            targetSlider.value = 0f;
        }
    }
}
