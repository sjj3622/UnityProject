using UnityEngine;
using UnityEngine.UI;

public class CPRSliderController : MonoBehaviour
{
    [Header("Slider Settings")]
    public Slider cprSlider;             // 슬라이더 UI
    public TimerController timerScript;  // TimerController 스크립트 참조

    [Header("Slider Range")]
    public float minValue = 0f;
    public float maxValue = 180f; // 타이머의 최대값 (timerDuration과 동일하게)

    [Header("Visual Settings")]
    public RectTransform fillMask;       // Fill을 가리는 Mask RectTransform
    public Image fillImage;              // Fill 이미지
    public Image handleImage;            // Handle 이미지

    private float fillFullWidth;

    void Start()
    {
        // 타이머 스크립트 자동 할당
        if (timerScript == null)
            timerScript = FindObjectOfType<TimerController>();

        // 슬라이더 자동 할당
        if (cprSlider == null)
            cprSlider = GetComponent<Slider>();

        // 기본 슬라이더 세팅
        cprSlider.minValue = minValue;
        cprSlider.maxValue = maxValue;

        // FillMask 초기 너비 저장
        if (fillMask != null)
            fillFullWidth = fillMask.sizeDelta.x;
    }

    void Update()
    {
        if (timerScript != null && cprSlider != null)
        {
            // TimerController의 totalTimer 값으로 슬라이더 갱신
            cprSlider.value = timerScript.totalTimer;

            // FillMask 너비 비율 계산 (0~1)
            float normalized = Mathf.InverseLerp(minValue, maxValue, cprSlider.value);

            if (fillMask != null)
            {
                Vector2 size = fillMask.sizeDelta;
                size.x = fillFullWidth * normalized; // FillMask 너비 변경
                fillMask.sizeDelta = size;
            }
        }
    }
}
