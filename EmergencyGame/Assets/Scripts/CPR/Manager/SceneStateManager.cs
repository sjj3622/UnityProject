using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    private Vector3 savedPlayerPosition;
    private Vector3 savedPatientPosition;
    private Vector3 savedCameraPosition;

    private bool playerSaved = false;
    private bool patientSaved = false;
    private bool cameraSaved = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
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

    // 씬 로드 후 복원
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "CPR1") // 특정 씬만 복원하고 싶다면 유지
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

        savedPlayerPosition = Vector3.zero;
        savedPatientPosition = Vector3.zero;
        savedCameraPosition = Vector3.zero;

        Debug.Log("저장된 상태 초기화됨.");
    }
}
