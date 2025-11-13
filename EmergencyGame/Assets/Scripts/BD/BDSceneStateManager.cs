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


    private bool playerSaved = false;
    private bool patientSaved = false;
    private bool cameraSaved = false;

    private bool TimerSaved = false;



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("소환");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("삭제");
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


        if (scene.name == "Bleeding") // 특정 씬만 복원하고 싶다면 유지
        {
            // Player 복원
            if (playerSaved)
            {
                Debug.Log("플레이어 복원");
                GameObject player = GameObject.Find("Player");
                if (player != null)
                {
                    player.transform.position = savedPlayerPosition;
                    Debug.Log("Player 위치 복원: " + savedPlayerPosition);
                }
            }

            // Patient 복원
            if (patientSaved)
            {
                Debug.Log("환자 복원");
                GameObject patient = GameObject.Find("Patient");
                if (patient != null)
                {
                    patient.transform.position = savedPatientPosition;
                    Debug.Log("Patient 위치 복원: " + savedPatientPosition);
                }
            }

            // Camera 복원
            if (cameraSaved)
            {
                Debug.Log("카메라 복원");
                Camera mainCam = Camera.main;
                if (mainCam != null)
                {
                    mainCam.transform.position = savedCameraPosition;
                    Debug.Log("카메라 위치 복원: " + savedCameraPosition);
                }
            }

            // 타이머 복원
            if (TimerSaved)
            {
                GameObject Timer = GameObject.Find("Timer");

                if (Timer != null)
                {
                    RectTransform timerRect = Timer.GetComponent<RectTransform>();
                    BDTimerController timerCtrl = Timer.GetComponent<BDTimerController>();

                    if (timerRect != null)
                    {
                        timerRect.localPosition = savedTimerPosition;
                    }

                    Timer.SetActive(true);
                    Debug.Log("Timer 위치 복원 (Canvas 기준): " + savedTimerPosition);
                }
            }

        }

    }





    public void SaveState(GameObject target)
    {
        if (target == null)
        {
            Debug.LogWarning("SaveState에 null이 들어왔습니다.");
            return;
        }

        string name = target.name;

        if (name == "Player")
        {
            savedPlayerPosition = target.transform.position;
            playerSaved = true;
            Debug.Log("Player 위치 저장: " + savedPlayerPosition);
        }
        else if (name == "Patient")
        {
            savedPatientPosition = target.transform.position;
            patientSaved = true;
            Debug.Log("Patient 위치 저장: " + savedPatientPosition);
        }
        else if (name.Contains("Camera"))
        {
            savedCameraPosition = target.transform.position;
            cameraSaved = true;
            Debug.Log("Camera 위치 저장: " + savedCameraPosition);
        }
        else if (name == "Timer")
        {
            RectTransform rect = target.GetComponent<RectTransform>();
            if (rect != null)
            {
                savedTimerPosition = rect.localPosition;
                TimerSaved = true;
                Debug.Log("Timer 위치 저장: " + savedTimerPosition);
            }
        }

        else
        {
            Debug.Log("SaveState: 대상이 Player, Patient, Camera가 아닙니다. 대상 이름: " + name);
        }
    }




    public void ClearSaved()
    {
        playerSaved = false;
        patientSaved = false;
        cameraSaved = false;
        TimerSaved = false;

        savedPlayerPosition = Vector3.zero;
        savedPatientPosition = Vector3.zero;
        savedCameraPosition = Vector3.zero;
        savedTimerPosition = Vector3.zero;



        Debug.Log("저장된 상태 초기화됨.");
    }










}