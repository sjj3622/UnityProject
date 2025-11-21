using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerGateManager : MonoBehaviour
{
    [Header("HouseFire 전용 설정")]
    public GameObject[] PlayerGates;  // 0~7 게이트
    public GameObject burnGate;       // HouseFire → Burn 이동에 사용되는 Gate(예: "playermove1-2")

    [Header("Burn 전용 설정")]
    public GameObject burnGate0;      // Burn 씬에서 HouseFire로 가는 Gate(예: "playermove1-1")

    [Header("공통 설정")]
    public float teleportOffset = 1f;
    public float teleportCooldown = 1f;

    private Dictionary<GameObject, GameObject> gateConnections = new Dictionary<GameObject, GameObject>();
    private HashSet<GameObject> cooldownGates = new HashSet<GameObject>();

    private string currentScene;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        // 플레이어 유지
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null) DontDestroyOnLoad(player);
    }

    private void Start()
    {
        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "HouseFire")
        {
            SetupHouseFireConnections();
        }
    }

    // ------------------------------
    // HouseFire 씬 게이트 연결 세팅
    // ------------------------------
    private void SetupHouseFireConnections()
    {
        gateConnections.Clear();

        // PlayerGates[0] = Burn 씬으로 가는 게이트
        gateConnections[PlayerGates[0]] = burnGate;

        // 내부 게이트 연결
        gateConnections[PlayerGates[1]] = PlayerGates[0];
        gateConnections[PlayerGates[2]] = PlayerGates[3];
        gateConnections[PlayerGates[3]] = PlayerGates[2];
        gateConnections[PlayerGates[4]] = PlayerGates[5];
        gateConnections[PlayerGates[5]] = PlayerGates[4];
        gateConnections[PlayerGates[6]] = PlayerGates[7];
        gateConnections[PlayerGates[7]] = PlayerGates[6];
    }

    // ------------------------------------------
    // 플레이어가 트리거에 들어왔을 때 (두 씬 공용)
    // ------------------------------------------
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        currentScene = SceneManager.GetActiveScene().name;

        if (currentScene == "Burn")
        {
            HandleBurnSceneTrigger(collision.gameObject);
        }
        else if (currentScene == "HouseFire")
        {
            HandleHouseFireTrigger(collision.gameObject);
        }
    }

    // ------------------------------
    // 🔥 Burn 씬 로직
    // ------------------------------
    private void HandleBurnSceneTrigger(GameObject player)
    {
        Debug.Log(cooldownGates.Contains(burnGate0));
        Debug.Log("충돌");
        if (cooldownGates.Contains(burnGate0)) return;

        StartCoroutine(TeleportBurnToHouseFire(player));
    }

    private IEnumerator TeleportBurnToHouseFire(GameObject player)
    {
        cooldownGates.Add(burnGate0);

        // HouseFire 씬 이동
        SceneManager.LoadScene("HouseFire");

        // 로드 기다림
        yield return new WaitUntil(() => GameObject.Find("playermove1-2") != null);

        GameObject gateInHouseFire = GameObject.Find("playermove1-2");

        if (gateInHouseFire != null)
        {
            player.transform.position = gateInHouseFire.transform.position + new Vector3(teleportOffset, 0, 0);

            // HouseFire 쪽 게이트도 쿨다운
            PlayerGateManager scriptInHouseFire = gateInHouseFire.GetComponentInParent<PlayerGateManager>();
            if (scriptInHouseFire != null)
            {
                scriptInHouseFire.StartCooldownTemp(gateInHouseFire);
            }
        }

        yield return new WaitForSeconds(teleportCooldown);

        cooldownGates.Remove(burnGate0);
    }

    // ------------------------------
    // 🔥 HouseFire 씬 로직
    // ------------------------------
    private void HandleHouseFireTrigger(GameObject player)
    {
        if (PlayerGates == null || PlayerGates.Length == 0) return;

        GameObject nearestGate = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject gate in PlayerGates)
        {
            if (gate == null) continue;
            float dist = Vector3.Distance(player.transform.position, gate.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearestGate = gate;
            }
        }

        if (nearestGate == null) return;
        if (cooldownGates.Contains(nearestGate)) return;

        GameObject targetGate = gateConnections[nearestGate];

        // Burn 씬으로 이동
        if (targetGate == burnGate)
        {
            StartCoroutine(TeleportHouseFireToBurn(player, nearestGate));
        }
        else
        {
            // 내부 게이트 이동
            player.transform.position = targetGate.transform.position + new Vector3(teleportOffset, 0, 0);
            StartCoroutine(StartCooldown(nearestGate));
        }
    }

    private IEnumerator TeleportHouseFireToBurn(GameObject player, GameObject gate)
    {
        cooldownGates.Add(gate);

        // Burn 씬 이동
        SceneManager.LoadScene("Burn");

        // Burn 씬에서 burnGate 이름으로 찾음
        yield return new WaitUntil(() => GameObject.Find(burnGate.name) != null);

        GameObject gateInBurn = GameObject.Find(burnGate.name);

        if (gateInBurn != null)
        {
            player.transform.position = gateInBurn.transform.position + new Vector3(teleportOffset, 0, 0);
        }

        yield return new WaitForSeconds(teleportCooldown);

        cooldownGates.Remove(gate);
    }

    // ------------------------------
    // 공통 쿨다운
    // ------------------------------
    private IEnumerator StartCooldown(GameObject gate)
    {
        cooldownGates.Add(gate);
        yield return new WaitForSeconds(teleportCooldown);
        cooldownGates.Remove(gate);
    }

    public void StartCooldownTemp(GameObject gate)
    {
        StartCoroutine(StartCooldown(gate));
    }
}
