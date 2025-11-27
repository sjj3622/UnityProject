using UnityEngine;

public class FallObject : MonoBehaviour
{
    HMTimerController hmtimerController;
    public Transform PlayerGate;

    void Start()
    {
        hmtimerController = FindAnyObjectByType<HMTimerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Fall과 충돌!");

        // Player가 아니면 무시
        if (!collision.CompareTag("Player"))
            return;

        Debug.Log("플레이어 맞음 → 타이머 감소 + 이동");

        // 타이머 20 감소
        hmtimerController.timer -= 20f;

        // PlayerGate 좌표로 플레이어 이동
        if (PlayerGate != null)
            collision.transform.position = PlayerGate.position;
    }
}
