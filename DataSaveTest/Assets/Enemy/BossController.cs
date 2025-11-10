using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class BossController : MonoBehaviour
{

    public int hp = 10;
    public float actionDist = 7.0f;

    public GameObject bulletFab;
    public float shootSpeed = 5.0f;

    bool inAttack = false;





    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0) return;


        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Vector3 prpos = player.transform.position;
        float dist = Vector2.Distance(transform.position, prpos);

        if (dist <= actionDist && !inAttack)
        {
            inAttack = true;
            GetComponent<Animator>().Play("BossAttack");


        }
        else if (dist > actionDist && inAttack)
        {
            inAttack = false;
            GetComponent<Animator>().Play("BossIdel");
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
                GetComponent<Animator>().Play("BossDead");
                Destroy(gameObject, 1.0f);

            }
        }
    }


    void Attack()
    {
        Transform tr = transform.Find("gate");
        GameObject gate = tr.gameObject;

        GameObject player = GameObject.FindGameObjectWithTag("Player");


        float dx = player.transform.position.x - gate.transform.position.x;
        float dy = player.transform.position.y - gate.transform.position.y;

        float rad = Mathf.Atan2(dy, dx);
        float angle = rad * Mathf.Rad2Deg;

        Quaternion r = Quaternion.Euler(0, 0, angle);
        GameObject bullet = Instantiate(bulletFab, gate.transform.position, r);


        float x = Mathf.Cos(rad);
        float y = Mathf.Sin(rad);
        Vector3 v = new Vector3(x, y) * shootSpeed;

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(v, ForceMode2D.Impulse);
    }
}
