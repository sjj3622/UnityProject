using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 3; //몬스터 생명력

    //몬스터 이동 관련
    public float speed = 2.2f;
    public float actionDist = 8.0f;
    public string idleAni = "EnemyIdle";
    public string upAni = "EnemyUp";
    public string downAni = "EnemyDown";
    public string leftAni = "EnemyLeft";
    public string rightAni = "EnemyRight";
    public string deadAni = "EnemyDead";


    string nowAni = "", oldAni = "";

    float axisH, axisV;
    Rigidbody2D rb;

    bool isActive = false; // 플레이어 감지하면 활성화
    public int arrangeld = 0;





    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (isActive)  //플레이어 감지
        {
            float dx = player.transform.position.x - transform.position.x;
            float dy = player.transform.position.y - transform.position.y;

            float rad = Mathf.Atan2(dy, dx);
            float angle = rad * Mathf.Rad2Deg;


            if (angle > -45.0f && angle <= 45.0f) nowAni = rightAni;
            else if (angle > 45.0f && angle <= 135.0f) nowAni = upAni;
            else if (angle > 135.0f && angle <= 225.0f) nowAni = leftAni;
            else nowAni = downAni;


            axisH = Mathf.Cos(rad) * speed;
            axisV = Mathf.Sin(rad) * speed;



        }
        else           // 플레이어 감지X
        {
            float dist = Vector2.Distance(transform.position, player.transform.position);
            if (dist < actionDist) isActive = true;   // 감지 거리안에 캐릭터 발견
        }
    }

    private void FixedUpdate()
    {
        if (isActive && hp > 0)
        {
            rb.velocity = new Vector2(axisH, axisV);
            if (nowAni != oldAni)
            {
                oldAni = nowAni;
                GetComponent<Animator>().Play(nowAni);

            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Arrow")
        {
            hp--;
            if (hp <= 0)
            {
                GetComponent<CircleCollider2D>().enabled = false;
                rb.velocity = new Vector2(0, 0);
                GetComponent<Animator>().Play(deadAni);
                Destroy(gameObject, 0.5f);

                SaveDataManager.SetArrangeId(arrangeld, gameObject.tag);
            }
        }
    }

}
