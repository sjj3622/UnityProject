using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private int fadeStep = 0; // 현재 투명화 단계 (0~3)

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Enemy가 맞았을 때 호출
    public void TakeHit()
    {
        fadeStep++; // 맞을 때마다 단계 증가

        switch (fadeStep)
        {
            case 1:
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.7f);
                break;
            case 2:
                spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
                break;
            case 3:
                spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
                Destroy(gameObject); // 3단계 되면 제거
                break;
        }
    }
}
