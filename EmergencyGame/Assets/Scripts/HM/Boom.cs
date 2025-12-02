using System.Collections;
using UnityEngine;

public class Boom : MonoBehaviour
{
    public GameObject explosionAreaGo;
    public CircleCollider2D circleCollider2D; // 폭발 범위용

    [Header("Animation Names")]
    public string bomb = "bomb";
    public string boob = "boob";

    private Animator animator;
    private bool exploded = false; // 폭발 중복 방지

    void Start()
    {
        animator = GetComponent<Animator>();
        explosionAreaGo.SetActive(false);

        StartCoroutine(ExplodeSequence());
    }

    IEnumerator ExplodeSequence()
    {
        // bomb 애니메이션 재생
        animator.Play(bomb);

        // 2초 대기
        yield return new WaitForSeconds(5f);

        // 자동 폭발
        TriggerExplosion();
    }

    void TriggerExplosion()
    {
        if (exploded) return;
        exploded = true;

        // boob 애니메이션 재생
        animator.Play(boob);

        // 폭발 범위 활성화
        explosionAreaGo.SetActive(true);

        // 폭발 범위 처리
        DestroyArea();

        // 오브젝트 제거 (1초 후)
        StartCoroutine(DestroyAfterDelay(1f));
    }

    IEnumerator DestroyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    void DestroyArea()
    {
        // Collider2D 범위 안의 Enemy 검색
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius, LayerMask.GetMask("Enemy"));

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();
                if (enemy != null)
                {
                    enemy.TakeHit(); // 맞을 때마다 1단계씩 투명화
                }
            }
        }

    }

    // 이 오브젝트가 Enemy와 충돌했을 때 즉시 폭발
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TriggerExplosion();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            TriggerExplosion();
        }
    }
}