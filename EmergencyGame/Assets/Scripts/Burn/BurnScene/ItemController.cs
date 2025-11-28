using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemIndex;


    public delegate void ItemCollected(int index, BPlayerController player);
    public event ItemCollected OnCollected;

    public delegate void PatientCollected(int index, PatientController patient);
    public event PatientCollected OnPatientCollected;

    private Camera mainCam;
    private float checkMargin = 0.1f; // 화면 밖 제거 여유값

    void Start()
    {
        mainCam = Camera.main;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var player = other.GetComponent<BPlayerController>();
        if (player != null)
        {
            OnCollected?.Invoke(itemIndex, player);
            Destroy(gameObject);
            return;
        }

        var patient = other.GetComponent<PatientController>();
        if (patient != null)
        {
            OnPatientCollected?.Invoke(itemIndex, patient);
            Destroy(gameObject);
        }
    }

    void Update()
    {
        // 화면 밖으로 나가면 제거
        if (mainCam == null) return;

        Vector3 viewPos = mainCam.WorldToViewportPoint(transform.position);
        if (viewPos.x < -checkMargin || viewPos.x > 1 + checkMargin ||
            viewPos.y < -checkMargin || viewPos.y > 1 + checkMargin)
        {
            Destroy(gameObject);
            Debug.Log("아이템 화면 밖 제거됨");
        }
    }
}
