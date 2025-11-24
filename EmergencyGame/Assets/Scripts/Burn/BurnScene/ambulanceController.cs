using UnityEngine;

public class AmbulanceController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject BPlayerPrefab;
    public GameObject AmbulancePrefab;

    private GameObject BPlayer;
    private GameObject Ambulance;

    [Header("Gate Positions")]
    public Transform StartGate;
    public Transform EndGate;

    [Header("Settings")]
    public float moveSpeed = 3.0f;

    BurnCanvas burnCanvas;
    PatientController patientController;

    public bool isEnding = false;


    public bool isMoving = true;

    private bool isBPlayerSpawned = false;

    private bool hasMovedBPlayer = false;

    void Start()
    {
        burnCanvas = FindAnyObjectByType<BurnCanvas>();
        
        SpawnAmbulance();
    }

    void Update()
    {


        if (BurngpManager.gameState == null && isMoving)
        {
            
            MoveAmbulance();
        }

        CheckGameState();
    }

    void SpawnAmbulance()
    {
        Debug.Log("Ambulance 소환");
        Ambulance = Instantiate(AmbulancePrefab, StartGate.position, Quaternion.identity);
        isMoving = true;
    }

    void SpawnBPlayer()
    {
        Debug.Log("BPlayer 소환 (EndGate 위치)");
        BPlayer = Instantiate(BPlayerPrefab, EndGate.position, Quaternion.identity);
        isBPlayerSpawned = true;
    }

    void MoveAmbulance()
    {
        if (Ambulance == null) return;

        Ambulance.transform.position = Vector3.MoveTowards(
            Ambulance.transform.position,
            EndGate.position,
            moveSpeed * Time.deltaTime
        );

        if (Vector3.Distance(Ambulance.transform.position, EndGate.position) < 0.1f)
        {
            isMoving = false;
            if (BurngpManager.gameState == null)
            {
                burnCanvas.selectPanel.SetActive(true);
            }

            if (!isBPlayerSpawned)
            {
                SpawnBPlayer();
            }
        }
    }

    void CheckGameState()
    {
        // Rescuer 상태일 때 Ambulance 시각적 플립 후 StartGate로 이동
        if (BurngpManager.gameState == "Rescuer" && Ambulance != null)
        {
            SpriteRenderer sr = Ambulance.GetComponent<SpriteRenderer>();
            if (sr != null && !sr.flipX)
            {
                sr.flipX = true;
            }

            Ambulance.transform.position = Vector3.MoveTowards(
                Ambulance.transform.position,
                StartGate.position,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(Ambulance.transform.position, StartGate.position) < 0.1f)
            {
                Destroy(Ambulance);
                Ambulance = null;
            }
        }

        // FireFighter 상태일 때 BPlayer 위치 이동
        if (BurngpManager.gameState == "FireFighter" && BPlayer != null && !hasMovedBPlayer)
        {
            BPlayer.transform.position = StartGate.position;
            hasMovedBPlayer = true;
        }

        // RescuerClear 상태일 때 처리
        if (BurngpManager.gameState == "RescuerClear")
        {

            // Ambulance가 없으면 StartGate에서 소환
            if (Ambulance == null)
            {
                Ambulance = Instantiate(AmbulancePrefab, StartGate.position, Quaternion.identity);

                //// x좌표 반전 (스프라이트 뒤집기)
                //SpriteRenderer sr = Ambulance.GetComponent<SpriteRenderer>();
                //if (sr != null)
                //{
                //    Debug.Log("sr.flipX" + sr.flipX);
                //    sr.flipX = false;
                //}
            }

            // EndGate까지 이동
            if (Ambulance != null && !isEnding)
            {
                patientController = FindAnyObjectByType<PatientController>();

                // 이동
                Ambulance.transform.position = Vector3.MoveTowards(
                    Ambulance.transform.position,
                    EndGate.position,
                    moveSpeed * Time.deltaTime
                );

                // 스프라이트 반전
                SpriteRenderer sr = Ambulance.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = false;
                }

                //EndGate에 도착했는지 확인
                if (Vector3.Distance(Ambulance.transform.position, EndGate.position) < 0.01f)
                {
                    if (patientController != null)
                    {
                        patientController.isarrive = true;
                    }
                }
            }


            if (isEnding)
            {
                SpriteRenderer sr = Ambulance.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.flipX = true;
                }

                // StartGate로 이동

                Ambulance.transform.position = Vector3.MoveTowards(
                    Ambulance.transform.position,
                    StartGate.position,
                    moveSpeed * Time.deltaTime
                );

                // StartGate 도착 시 제거
                if (Vector3.Distance(Ambulance.transform.position, StartGate.position) < 0.1f)
                {
                    Destroy(Ambulance);
                    Ambulance = null;
                }
            }
        }
    }

}




