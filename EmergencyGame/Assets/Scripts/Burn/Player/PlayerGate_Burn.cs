using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGate_Burn : MonoBehaviour
{
    public string houseFireGateName = "playermove1-2";
    public GameObject burnGate0;
    private float offsetX = -1f;
    public float cooldownTime = 1f;

    private HashSet<GameObject> cooldownSet = new HashSet<GameObject>();
    private GameObject playerToTeleport;

    public static AmbulanceController Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);   // 중복이면 제거
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag("Player")) return;
        if (cooldownSet.Contains(burnGate0)) return;
        if (BurngpManager.gameState != "FireFighter") return;

        playerToTeleport = col.gameObject;
        cooldownSet.Add(burnGate0);

        BurngpManager.gameState = "FFStart";

        // 씬 로드 후 이벤트 구독
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("HouseFire");
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "HouseFire") return;

        GameObject gateInHouseFire = GameObject.Find(houseFireGateName);
        if (gateInHouseFire != null && playerToTeleport != null)
        {
            Vector3 offset = new Vector3(offsetX, 2f, 0);
            playerToTeleport.transform.position = gateInHouseFire.transform.position + offset;
            Debug.Log("Firer 이동 :" + playerToTeleport.transform.position);
        }

        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 쿨다운 제거
        StartCoroutine(RemoveCooldown());
    }

    private IEnumerator RemoveCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldownSet.Remove(burnGate0);
    }
}
