using UnityEngine;

public class BloodObject : MonoBehaviour
{
    public enum ClickType
    {
        SingleClick,
        DoubleClick,
        Hold
    }

    [Header("클릭 가능 여부")]
    public bool removableByClick = true;

    [Header("필요한 커서 타입")]
    public int requiredMouseType = 0; // blood1=0, blood2=1, blood3=2, blood4=3

    [Header("클릭 방식")]
    public ClickType clickType = ClickType.SingleClick;

    [Header("홀드 클릭 시간 (초)")]
    public float holdTimeRequired = 0f;

    [Header("점수")]
    public int score = 1;

    private float holdTimer = 0f;
    private bool isMouseOver = false;
    private float lastClickTime = 0f;
    private int clickCount = 0;

    void Awake()
    {
        // 이름으로 타입 설정
        if (name.Contains("blood1"))
        {
            requiredMouseType = 0;
            clickType = ClickType.SingleClick;
            score = 1;
        }
        else if (name.Contains("blood2"))
        {
            requiredMouseType = 1;
            clickType = ClickType.DoubleClick;
            score = 3;
        }
        else if (name.Contains("blood3"))
        {
            requiredMouseType = 2;
            clickType = ClickType.Hold;
            holdTimeRequired = 2f;
            score = 5;
        }
        else if (name.Contains("blood4"))
        {
            requiredMouseType = 3;
            clickType = ClickType.Hold;
            holdTimeRequired = 5f;
            score = 10;
        }
    }

    void Update()
    {
        if (!removableByClick || !isMouseOver) return;

        Mouse mouse = FindAnyObjectByType<Mouse>();
        if (mouse == null) return;

        if (mouse.mouseImg != requiredMouseType)
        {
            ResetHold();
            return;
        }

        switch (clickType)
        {
            case ClickType.SingleClick:
                if (Input.GetMouseButtonDown(0))
                    RemoveObject();
                break;

            case ClickType.DoubleClick:
                if (Input.GetMouseButtonDown(0))
                {
                    clickCount++;
                    if (clickCount == 1)
                        lastClickTime = Time.time;

                    if (clickCount == 2 && (Time.time - lastClickTime) <= 0.3f)
                    {
                        RemoveObject();
                        clickCount = 0;
                    }
                }

                // 0.3초 안에 두 번째 클릭 없으면 리셋
                if (clickCount == 1 && (Time.time - lastClickTime) > 0.3f)
                    clickCount = 0;
                break;

            case ClickType.Hold:
                if (Input.GetMouseButton(0))
                {
                    holdTimer += Time.deltaTime;
                    if (holdTimer >= holdTimeRequired)
                        RemoveObject();
                }
                else
                {
                    ResetHold();
                }
                break;
        }
    }

    void RemoveObject()
    {
        BDScoreController scoreController = FindAnyObjectByType<BDScoreController>();
        if (scoreController != null)
            scoreController.AddScore(score);

        Destroy(gameObject);
        Debug.Log($"{gameObject.name} 제거됨 (점수 {score}, 클릭타입 {clickType})");
    }

    void ResetHold()
    {
        holdTimer = 0f;
    }

    void OnMouseEnter() => isMouseOver = true;
    void OnMouseExit()
    {
        isMouseOver = false;
        ResetHold();
        clickCount = 0;
    }

    // BloodObject.cs 안에 추가
    public void OnClicked(int currentMouseType)
    {
        if (!removableByClick) return;
        if (currentMouseType != requiredMouseType)
        {
            ResetHold();
            clickCount = 0;
            return;
        }

        switch (clickType)
        {
            case ClickType.SingleClick:
                RemoveObject();
                break;

            case ClickType.DoubleClick:
                clickCount++;
                if (clickCount == 1)
                    lastClickTime = Time.time;

                if (clickCount == 2 && (Time.time - lastClickTime) <= 0.3f)
                {
                    RemoveObject();
                    clickCount = 0;
                }

                if (clickCount == 1 && (Time.time - lastClickTime) > 0.3f)
                    clickCount = 0;
                break;

            case ClickType.Hold:
                // 홀드 타입은 Mouse에서 호출로는 바로 처리 불가
                // 필요한 경우 Update()에서 기존 처리 유지
                break;
        }
    }

}
