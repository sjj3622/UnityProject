using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class WindowSetting : MonoBehaviour
{
    const int GWL_STYLE = -16;
    const int WS_SIZEBOX = 0x00040000;  // 드래그 가능
    const int WS_MAXIMIZEBOX = 0x00010000;
    const int WS_MINIMIZEBOX = 0x00020000;

    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    static extern int GetWindowLong(IntPtr hWnd, int nIndex);

    [DllImport("user32.dll")]
    static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

    void Start()
    {
        IntPtr window = GetActiveWindow();

        int style = GetWindowLong(window, GWL_STYLE);

        style |= WS_SIZEBOX;       // 사이즈 조절 허용
        style |= WS_MAXIMIZEBOX;   // 최대화 버튼 활성
        style |= WS_MINIMIZEBOX;   // 최소화 버튼 활성

        SetWindowLong(window, GWL_STYLE, style);
    }
}
