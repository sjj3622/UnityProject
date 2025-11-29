using System;
using UnityEngine;

public class AmbulanceController : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject BPlayerPrefab;
    public GameObject FireFighterfab;
    public GameObject AmbulancePrefab;

    private GameObject FireFighter;
    private GameObject BPlayer;
    private GameObject Ambulance;

    [Header("Gate Positions")]
    public Transform StartGate;
    public Transform EndGate;

    [Header("Settings")]
    public float moveSpeed = 3.0f;

    

    BurnCanvas burnCanvas;
    PatientController patientController;
    FFPlayerController ffPlayerController;

    public bool isEnding = false;

    public bool isClear = false;

    public bool isMoving = true;

    private bool isBPlayerSpawned = false;

    private bool hasMovedBPlayer = false;

    void Start()
    {
        ffPlayerController = FindAnyObjectByType<FFPlayerController>();
        
        burnCanvas = FindAnyObjectByType<BurnCanvas>();
        
        SpawnAmbulance();
    }

    void Update()
    {

        if (ffPlayerController != null)
        {
            FireFighter = ffPlayerController.gameObject;
        }
        //else
        //{
        //    Debug.LogWarning("씬에 FFPlayerController가 없습니다!");
        //}

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
        if ((BurngpManager.gameState == "FireFighter"|| BurngpManager.gameState == "FireFighterClear") && FireFighter != null &&!isClear)
        {
            Ambulance.transform.position = EndGate.position;

        }

        // RescuerClear 상태일 때 처리
        if (BurngpManager.gameState == "RescuerClear" || BurngpManager.gameState == "FireFighterClear")
        {
            
            if (GameObject.FindWithTag("Item") == null && Ambulance == null)
            {
                Debug.Log("Item이 없으므로 Ambulance 소환");
                Ambulance = Instantiate(AmbulancePrefab, StartGate.position, Quaternion.identity);
            }
            
            // EndGate까지 이동
            if (Ambulance != null && !isEnding && BurngpManager.gameState == "RescuerClear")
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

    public static implicit operator AmbulanceController(PlayerGate_Burn v)
    {
        throw new NotImplementedException();
    }
}




