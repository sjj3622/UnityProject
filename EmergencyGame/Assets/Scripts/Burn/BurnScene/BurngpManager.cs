using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BurngpManager : MonoBehaviour
{
    ItemDropController itemDropController;
    SmokeGateController smokeGateController;
    BurnTimerController burnTimerController;
    AmbulanceController ambulanceController;
    

    public GameObject patient;
    public GameObject fireFighterPrefab;
    public GameObject ClearPanel;
    public GameObject IconPanel;
    
    public TextMeshProUGUI GameOverText;


    public static string gameState;

    private bool ispatient = false;
    private bool isPanel = false;


    private float moveSpeed = 5f;  // fireFighter 이동 속도

    private void Start()
    {
        itemDropController = FindAnyObjectByType<ItemDropController>();
        smokeGateController = FindAnyObjectByType<SmokeGateController>();
        burnTimerController = FindAnyObjectByType<BurnTimerController>();
        ambulanceController = FindAnyObjectByType<AmbulanceController>();
        
        ClearPanel.SetActive(false);
        IconPanel.SetActive(false);
        
        GameOverText.text = "";
    }

    void Update()
    {
        if (gameState == "BOver")
        {
            Debug.Log("게임 끝");
            GameOverText.text = "GAME OVER";
            burnTimerController.timerRunning = false;
            itemDropController.gameObject.SetActive(false);
            smokeGateController.gameObject.SetActive(false);
            IconPanel.SetActive(false);
            StartCoroutine(wait());

        }


        if (gameState == "Rescuer" && !ispatient)
        {
            StartCoroutine(SpawnSequence());
            ispatient = true;
            
        }
        if (gameState == "RescuerClear")
        {
            
            burnTimerController.timerRunning = false;
            itemDropController.gameObject.SetActive(false);
            smokeGateController.gameObject.SetActive(false);
            IconPanel.SetActive(false);

            if (GameObject.FindWithTag("Ambulance") == null && isPanel)
            {
                ClearPanel.SetActive(true);
            }
            isPanel = true;

        }
        

    }
    private IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        gameState = null;
        Debug.Log(gameState);
        SceneManager.LoadScene("Title");
    }
    private IEnumerator SpawnSequence()
    {
        // 1. playermove1-1 찾기
        GameObject playermove = GameObject.Find("playermove1-1");
        if (playermove == null) yield break;

        // 2. BPlayer 기준 X+1 위치에 fireFighterPrefab 생성
        GameObject BPlayer = GameObject.Find("BPlayer(Clone)");
        if (BPlayer == null) yield break;
        Vector3 fireFighterSpawnPos = BPlayer.transform.position + Vector3.right * 1.0f;
        GameObject fireFighter = Instantiate(fireFighterPrefab, fireFighterSpawnPos, Quaternion.identity);

        // 3. fireFighterPrefab을 playermove1-1 위치로 부드럽게 이동
        Vector3 targetPos = playermove.transform.position;
        while (Vector3.Distance(fireFighter.transform.position, targetPos) > 0.01f)
        {
            fireFighter.transform.position = Vector3.MoveTowards(fireFighter.transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        // 4. 이동 완료 후 fireFighter 제거
        Destroy(fireFighter);

        // 5. 3초 대기
        yield return new WaitForSeconds(3f);

        // 6. patient와 fireFighterPrefab을 플레이어 앞 위치(-1 방향)에 소환
        Vector3 frontPos = playermove.transform.position + playermove.transform.right * -1.0f;
        Instantiate(patient, frontPos, playermove.transform.rotation);
        GameObject fireFighter2 = Instantiate(fireFighterPrefab, frontPos, Quaternion.identity);

        // 7. 1초 대기 후 fireFighterPrefab 제거
        yield return new WaitForSeconds(1f);
        Destroy(fireFighter2);
    }

}