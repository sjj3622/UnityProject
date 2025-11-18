using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public class VideoOption : MonoBehaviour
{
    FullScreenMode screenMode;

    public Dropdown resolutionDropdown;
    public Toggle fullscreenBtn;
    List<Resolution>resolutions = new List<Resolution>();

    int resolutionNum;
    void Start()
    {
        InitUI();
    }

    void InitUI()
    {
        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            if (Screen.resolutions[i].refreshRateRatio.value >= 143f && Screen.resolutions[i].refreshRateRatio.value <= 145f)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }


        //resolutions.AddRange(Screen.resolutions);
        resolutionDropdown.options.Clear();

        int optionNum = 0;
        foreach (Resolution item in resolutions) 
        {
            Dropdown.OptionData option = new Dropdown.OptionData();
            option.text = item.width + "X" + item.height + "," + item.refreshRateRatio + "hz";
            resolutionDropdown.options.Add(option);

            if (item.width == Screen.width && item.height == Screen.height)
                resolutionDropdown.value = optionNum;
            optionNum++;
        }

        resolutionDropdown.RefreshShownValue();

        fullscreenBtn.isOn = Screen.fullScreenMode.Equals(FullScreenMode.FullScreenWindow)?true:false;
    }
    
    public void DropboxOptionChange(int x)
    {
        resolutionNum = x;
    }
    public void FullScreenBtn(bool isFull)
    {
        screenMode = isFull?FullScreenMode.FullScreenWindow:FullScreenMode.Windowed;
    }


    public void OKBtnClick()
    {
        Screen.SetResolution(resolutions[resolutionNum].width,
            resolutions[resolutionNum].height, screenMode);
    }
}
