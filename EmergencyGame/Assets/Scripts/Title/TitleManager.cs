using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public GameObject StagePanel;
    public GameObject PlayerPanel;
    public GameObject SettingPanel;
    public GameObject SettingsBtn;
    public GameObject PlayBtn;
    public GameObject LevelsBtn;
    public GameObject ExitBtn;
    public GameObject backLight;
    public GameObject backNight;
    public WindowSetting camSize;

    private Dictionary<GameObject, Vector2> originalPositions = new Dictionary<GameObject, Vector2>();
    private Dictionary<GameObject, bool> isMoved = new Dictionary<GameObject, bool>();

    private Image panelImage;

    void Start()
    {
        StagePanel.SetActive(false);
        PlayerPanel.SetActive(false);
        SettingPanel.SetActive(false);

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
            pos.x = 0f;
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

    public void SettingClick()
    {
        if (!SettingPanel.activeSelf) SettingPanel.SetActive(true);
        else SettingPanel.SetActive(false);

        PlayerPanel.SetActive(false);

    }

    public void ScreenSize()
    {
        if (!Screen.fullScreen)
        {
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreen = false;
        }
    }


    public void LevelsClick()
    {
        //MoveButton(PlayBtn);
        SettingPanel.SetActive(false);

        if (PlayBtn.activeSelf)
            PlayBtn.SetActive(false);
        else PlayBtn.SetActive(true);

        if (PlayerPanel.activeSelf)
        {
            PlayerPanel.SetActive(false);
        }

        MoveButton(LevelsBtn);
        MoveButton(ExitBtn);

        if (SettingsBtn.activeSelf)
        {
            SettingsBtn.SetActive(false);
        }
        else
        {
            SettingsBtn.SetActive(true);
        }

    }

    public void PlayClick()
    {

        if (!PlayerPanel.activeSelf) PlayerPanel.SetActive(true);
        else PlayerPanel.SetActive(false);

        SettingPanel.SetActive(false);
    }

    public void ExitClick()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }
    void OnApplicationQuit()
    {
        if (GameDataManager.Instance != null)
            GameDataManager.Instance.UploadGameData();
    }


    public void CPRClick() => SceneManager.LoadScene("CPR");
    public void HMClick() => SceneManager.LoadScene("HM");
    public void BleedingClick() => SceneManager.LoadScene("Bleeding");
    public void BurnClick() => SceneManager.LoadScene("Burn");
}