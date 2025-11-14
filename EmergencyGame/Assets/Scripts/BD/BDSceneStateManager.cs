using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BDSceneStateManager : MonoBehaviour
{
    public static BDSceneStateManager instance;

    private Vector3 savedPlayerPosition;
    private Vector3 savedPatientPosition;
    private Vector3 savedCameraPosition;
    private Vector3 savedTimerPosition;

    private float savedCameraSize;

    private float savedTimerValue;
    private bool savedTimerRunning;

    private bool playerSaved = false;
    private bool patientSaved = false;
    private bool cameraSaved = false;
    private bool TimerSaved = false;
    private bool timerValueSaved = false;
    private bool timerRunningSaved = false;
    private bool cameraSizeSaved = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("StateManager 생성");
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded 호출됨: {scene.name}");

        if (scene.name == "Bleeding")
        {
            // --- Player 복원 ---
            if (playerSaved)
            {
                GameObject player = GameObject.Find("Player");
                if (player != null)
                    player.transform.position = savedPlayerPosition;
            }

            // --- Patient 복원 ---
            if (patientSaved)
            {
                GameObject patient = GameObject.Find("Patient");
                if (patient != null)
                    patient.transform.position = savedPatientPosition;
            }

            // --- Camera 복원 ---
            if (cameraSaved)
            {
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    mainCam.transform.position = savedCameraPosition;

                    if (cameraSizeSaved)
                        mainCam.orthographicSize = savedCameraSize;
                }
            }

            // --- Timer 복원 ---
            if (TimerSaved)
            {
                GameObject Timer = GameObject.Find("Timer");

                if (Timer != null)
                {
                    RectTransform timerRect = Timer.GetComponent<RectTransform>();
                    BDTimerController timerCtrl = Timer.GetComponent<BDTimerController>();

                    // 위치 복원
                    if (timerRect != null)
                        timerRect.localPosition = savedTimerPosition;

                    // 시간 복원
                    if (timerValueSaved && timerCtrl != null)
                        timerCtrl.SetCurrentTime(savedTimerValue);

                    // 실행 여부 복원
                    if (timerRunningSaved && timerCtrl != null)
                        timerCtrl.SetTimerRunning(savedTimerRunning);

                    Timer.SetActive(true);
                }
            }
        }
    }

    // ──────────────────────────────────────────────
    //                ◼ SAVE STATE ◼
    // ──────────────────────────────────────────────
    public void SaveState(GameObject target)
    {
        if (target == null) return;

        string name = target.name;

        if (name == "Player")
        {
            savedPlayerPosition = target.transform.position;
            playerSaved = true;
        }
        else if (name == "Patient")
        {
            savedPatientPosition = target.transform.position;
            patientSaved = true;
        }
        else if (name.Contains("Camera"))
        {
            Camera cam = target.GetComponent<Camera>();
            if (cam != null)
            {
                savedCameraPosition = target.transform.position;
                savedCameraSize = cam.orthographicSize;

                cameraSaved = true;
                cameraSizeSaved = true;
            }
        }
        else if (name == "Timer")
        {
            RectTransform rect = target.GetComponent<RectTransform>();
            BDTimerController timerCtrl = target.GetComponent<BDTimerController>();

            // 위치 저장
            if (rect != null)
            {
                savedTimerPosition = rect.localPosition;
                TimerSaved = true;
            }

            // 시간 저장
            if (timerCtrl != null)
            {
                savedTimerValue = timerCtrl.GetCurrentTime();
                timerValueSaved = true;

                savedTimerRunning = timerCtrl.timerRunning;
                timerRunningSaved = true;
            }
        }
    }

    // ──────────────────────────────────────────────
    //                ◼ CLEAR STATE ◼
    // ──────────────────────────────────────────────
    public void ClearSaved()
    {
        playerSaved = false;
        patientSaved = false;
        cameraSaved = false;
        TimerSaved = false;
        timerValueSaved = false;
        timerRunningSaved = false;

        savedPlayerPosition = Vector3.zero;
        savedPatientPosition = Vector3.zero;
        savedCameraPosition = Vector3.zero;
        savedTimerPosition = Vector3.zero;

        Debug.Log("저장된 상태 모두 초기화됨.");


        GameObject timerObj = GameObject.Find("Timer");
        if (timerObj != null)
        {
            BDTimerController timerCtrl = timerObj.GetComponent<BDTimerController>();

            if (timerCtrl != null)
            {
                // 기본 시간으로 되돌림 (예: 180초)
                timerCtrl.SetCurrentTime(timerCtrl.timerDuration);

                // 타이머 정지
                timerCtrl.SetTimerRunning(false);
            }
        }
    }
}
