using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject StagePanel;
    public GameObject PlayBtn;
    public GameObject LevelsBtn;
    public GameObject ExitBtn;
    public GameObject backLight;
    public GameObject backNight;

    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    private Dictionary<GameObject, bool> isMoved = new Dictionary<GameObject, bool>();

    private Image panelImage;

    void Start()
    {
        StagePanel.SetActive(false);
        panelImage = StagePanel.GetComponent<Image>();

        SaveOriginalPosition(PlayBtn);
        SaveOriginalPosition(LevelsBtn);
        SaveOriginalPosition(ExitBtn);
    }

    void SaveOriginalPosition(GameObject button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        if (rect != null)
        {
            originalPositions[button] = rect.anchoredPosition;
            isMoved[button] = false;
        }
    }

    void MoveButton(GameObject button)
    {
        RectTransform rect = button.GetComponent<RectTransform>();
        if (rect == null) return;

        if (!isMoved[button])
        {
            Vector3 pos = rect.anchoredPosition;
            pos.x = 250f;
            rect.anchoredPosition = pos;

            StagePanel.SetActive(true);

            //투명하게 만들기 (RGBA)
            if (panelImage != null)
                panelImage.color = new Color(0f, 0f, 0f, 0.5f); // 검정 반투명 (50%)

            isMoved[button] = true;
            backLight.SetActive(false);
            backNight.SetActive(true);

        }
        else
        {
            rect.anchoredPosition = originalPositions[button];

            StagePanel.SetActive(false);

            isMoved[button] = false;
            backNight.SetActive(false);
            backLight.SetActive(true);
        }
    }

    public void LevelsClick()
    {
        MoveButton(PlayBtn);
        MoveButton(LevelsBtn);
        MoveButton(ExitBtn);
    }

    public void ExitClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void CPRClick() => SceneManager.LoadScene("CPR");
    public void FMClick() => SceneManager.LoadScene("FM");
    public void BleedingClick() => SceneManager.LoadScene("Bleeding");
    public void BurnClick() => SceneManager.LoadScene("Burn");
}
