using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    private Vector3 savedPlayerPosition;
    private Vector3 savedPatientPosition;
    private Vector3 savedCameraPosition;
    private Vector3 savedTimerPosition;

    
    public string savedTimerText = "";

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

    // 씬 로드 후 복원
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded 호출됨: {scene.name}");


        if (scene.name == "CPR") // 특정 씬만 복원하고 싶다면 유지
        {
            // Player 복원
            if (playerSaved)
            {
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
                    TimerController timerCtrl = Timer.GetComponent<TimerController>();

                    if (timerRect != null)
                    {
                        timerRect.localPosition = savedTimerPosition;
                    }

                    if (timerCtrl != null && timerCtrl.timerText != null)
                    {
                        timerCtrl.timerText.text = savedTimerText; // 텍스트 복원
                        Debug.Log("Timer 텍스트 복원: " + savedTimerText);
                    }

                    Timer.SetActive(true);
                    Debug.Log("Timer 위치 복원 (Canvas 기준): " + savedTimerPosition);
                }
            }


        }
        if (scene.name == "GamePlaying") // 특정 씬만 복원하고 싶다면 유지
        {
            if (TimerSaved)
            {
                GameObject Timer = GameObject.Find("Timer");
                if (Timer == null)
                {
                    Debug.LogWarning("Timer 오브젝트가 씬에 없습니다.");
                    return;
                }

                // Canvas 찾기
                GameObject canvas = GameObject.Find("Canvas");
                if (canvas == null)
                {
                    Debug.LogWarning("Canvas를 찾을 수 없습니다. Timer를 표시할 수 없습니다.");
                    return;
                }

                // Canvas 하위로 재설정
                Timer.transform.SetParent(canvas.transform, false);
                Timer.SetActive(true);

                // 위치 및 텍스트 복원
                RectTransform timerRect = Timer.GetComponent<RectTransform>();
                TimerController timerCtrl = Timer.GetComponent<TimerController>();

                if (timerRect != null)
                    timerRect.localPosition = savedTimerPosition;

                if (timerCtrl != null && timerCtrl.timerText != null)
                    timerCtrl.timerText.text = savedTimerText;

                Debug.Log("Timer 위치 및 텍스트 복원 완료.");
            }

        }

    }

    // 저장 함수 (Player / Patient / Camera 자동 인식)
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
            RectTransform timerRect = target.GetComponent<RectTransform>();
            TimerController timerCtrl = target.GetComponent<TimerController>();

            if (timerRect != null)
            {
                savedTimerPosition = timerRect.localPosition; // 위치 저장
                TimerSaved = true;
                Debug.Log("Timer 위치 저장 (Canvas 기준): " + savedTimerPosition);
            }

            if (timerCtrl != null && timerCtrl.timerText != null)
            {
                savedTimerText = timerCtrl.timerText.text; // 텍스트 저장
                Debug.Log("Timer 텍스트 저장: " + savedTimerText);
            }
        }
        else
        {
            Debug.Log("SaveState: 대상이 Player, Patient, Camera가 아닙니다. 대상 이름: " + name);
        }
    }

    // 전부 저장
    public void SaveAll()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            savedPlayerPosition = player.transform.position;
            playerSaved = true;
        }

        GameObject patient = GameObject.Find("Patient");
        if (patient != null)
        {
            savedPatientPosition = patient.transform.position;
            patientSaved = true;
        }

        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            savedCameraPosition = mainCam.transform.position;
            cameraSaved = true;
        }

        Debug.Log("Player, Patient, Camera 위치 저장 완료");
    }

    // 초기화
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


        savedTimerText = "";

        Debug.Log("저장된 상태 초기화됨.");
    }


}