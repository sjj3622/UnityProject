using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 2;



    void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(collision.transform);
        GetComponent<BoxCollider2D>().enabled = false; // 충돌 판정 비활성화
        GetComponent<Rigidbody2D>().simulated = false; // 물리 심뮬레이션 비활성화
    }


    void Update()
    {
        
    }
}
