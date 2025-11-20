using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    public static VideoOption Instance;
    FullScreenMode screenMode;

    public Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution> resolutions = new List<Resolution>();

    int resolutionNum;
    public static bool screenstart = false;


    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {

        if (!screenstart)
        {
            // 초기 전체화면 모드와 해상도 설정
            screenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, screenMode);
            screenstart = true;
        }

        InitUI();

        Debug.Log("초기 스크린 너비: " + Screen.width);
        Debug.Log("초기 스크린 높이: " + Screen.height);
        Debug.Log("초기 전체화면 모드: " + Screen.fullScreenMode);
    }

    void InitUI()
    {
        resolutions.Clear();

        // 안정적인 Windows 빌드용 해상도 목록
        resolutions.Add(new Resolution { width = 1280, height = 720, refreshRate = 60 });
        resolutions.Add(new Resolution { width = 1920, height = 1080, refreshRate = 60 });
        resolutions.Add(new Resolution { width = 1600, height = 900, refreshRate = 60 });

        resolutionDropdown.options.Clear();

        for (int i = 0; i < resolutions.Count; i++)
        {
            var item = resolutions[i];
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "x" + item.height + " @" + item.refreshRate + "Hz";
            resolutionDropdown.options.Add(option);

            // 기본 해상도 1280x720 선택
            if (item.width == 1280 && item.height == 720)
            {
                resolutionDropdown.value = i;
                resolutionNum = i;
            }
        }

        resolutionDropdown.RefreshShownValue();

        // 전체화면 토글 초기값
        fullscreenBtn.isOn = (screenMode == FullScreenMode.ExclusiveFullScreen);
    }

    // Dropdown 선택 시 호출
    public void DropboxOptionChange(int index)
    {
        resolutionNum = index;
    }

    // 전체화면 토글 시 호출
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
    }

    // OK 버튼 클릭 시 호출
    public void OKBtnClick()
    {
        Resolution selectedRes = resolutions[resolutionNum];
        Screen.SetResolution(selectedRes.width, selectedRes.height, screenMode);

        Debug.Log("적용된 해상도: " + selectedRes.width + "x" + selectedRes.height);
        Debug.Log("적용된 전체화면 모드: " + screenMode);
    }

    // 옵션 확인용 업데이트
    //void Update()
    //{
    //    Debug.Log("업데이트 스크린 너비: " + Screen.width);
    //    Debug.Log("업데이트 스크린 높이: " + Screen.height);
    //    Debug.Log("업데이트 전체화면 모드: " + Screen.fullScreenMode);
    //}
}
