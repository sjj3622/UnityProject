using UnityEngine;

public class ItemController : MonoBehaviour
{
    public int itemIndex; // DropController에서 설정될 아이템 인덱스

    // 플레이어 수집 이벤트
    public delegate void ItemCollected(int index, BPlayerController player);
    public event ItemCollected OnCollected;

    // 환자 수집 이벤트
    public delegate void PatientCollected(int index, PatientController patient);
    public event PatientCollected OnPatientCollected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어 충돌 체크
        BPlayerController player = other.GetComponent<BPlayerController>();
        if (player != null)
        {
            OnCollected?.Invoke(itemIndex, player);
            Destroy(gameObject);
            return; // 이미 처리했으면 함수 종료
        }

        // 환자 충돌 체크
        PatientController patient = other.GetComponent<PatientController>();
        if (patient != null)
        {
            OnPatientCollected?.Invoke(itemIndex, patient);
            Destroy(gameObject);
        }
    }
}
