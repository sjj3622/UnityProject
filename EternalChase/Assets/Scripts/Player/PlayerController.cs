<<<<<<< HEAD
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
=======
﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor.Tilemaps;
using UnityEngine;
using UnityEngine.Audio;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
<<<<<<< HEAD
    public float speed = 3.0f; // �̵� �ӵ�

    float axisH, axisV;
    public static string gameState = "playing";

    //SPUM ������  �ִϸ��̼� �����
    SPUM_Prefabs spum;
    Vector2 lastMoveDir = Vector2.down;  // ������ �ٷκ� ���� ǥ��
    const float moveDeadZone = 0.05f; // �̼������� ����
    private PlayerState currentState = PlayerState.IDLE;

    float baseScaleX = 1f;
    float lastSign = -1f;
    Transform visual;

    private void Awake()
    {
        spum = GetComponent<SPUM_Prefabs>();
        spum.OverrideControllerInit();
        visual = spum._anim.transform;
    }
=======
    public float speed = 3.0f; //이동속도
    public string upAni = "PlayerUp";
    public string downAni = "PlayerDown";
    public string leftAni = "PlayerLeft";
    public string rightAni = "PlayerRight";
    public string deadAni = "PlayerDead";

    string nowAni = ""; //현재 애니메이션
    string oldAni = ""; //이전 애니메이션

    float axisH, axisV;
    public float angleZ = -90.0f; // 회전각

    bool isMoving = false; //이동여부

    public static int hp = 3; //캐릭터 생명력
    public static string gameState; //게임 상태 -게임중(playing),게임오버(gameOver) 
    bool inDamage = false; //데미지 여부




>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        //gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("gameState ::::::" + gameState );

        if (gameState == "Battle")
        {
            currentState = PlayerState.IDLE; // ������ ����
            return;
        }


        if (gameState != "playing") return; 
                                                       
      
         axisH = Input.GetAxisRaw("Horizontal");  // �¿� �̵�Ű
         axisV = Input.GetAxisRaw("Vertical"); // ���� �̵�Ű


        if (moveDeadZone < Mathf.Abs(axisH))
        {
            //�¿� �̵� ����
            lastSign = axisH > 0 ? -1f : 1f;
            var s = visual.localScale;
            s.x = baseScaleX * lastSign;
            visual.localScale = s;
        }


        Vector2 input;
        input.x= axisH;
        input.y= axisV;
        input.Normalize();

        if (input.magnitude > 0.01f)
            currentState = PlayerState.MOVE;
        else
            currentState = PlayerState.IDLE;

        if (Input.GetKeyDown(KeyCode.Space))
            currentState = PlayerState.ATTACK;

            var animList = spum.StateAnimationPairs[currentState.ToString()];
        int index = 0;
        spum.PlayAnimation(currentState, index);
=======
        oldAni = downAni;
        gameState = "playing";
        hp = PlayerPrefs.GetInt("PlayerHP");




    }






    void Update()
    {
        if (gameState != "playing" || inDamage) return; // 게임상태가 플레이중이 아니거나
                                                        // 데미지 입고 있는중이면 이동 못함
        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal");  // 좌우 이동키
            axisV = Input.GetAxisRaw("Vertical");    // 상하 이동키
        }

        //키입력으로 각도를 구하고 각도에 따라 애니메이션 변경
        Vector2 fromPt = transform.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);

        angleZ = GetAngle(fromPt, toPt);

        if (angleZ >= -45 && angleZ < 45)
        {
            nowAni = rightAni;
        }
        else if (angleZ >= 45 && angleZ <= 135)
        {
            nowAni = upAni;
        }
        else if (angleZ > 135 && angleZ < 225)
        {
            nowAni = leftAni;
        }
        else
        {
            nowAni = downAni;
        }

        // 변경 애니메이션 적용
        if (nowAni != oldAni)
        {
            oldAni = nowAni;
            //GetComponent<Animator>().Play(nowAni);
        }
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea

    }

    private void FixedUpdate()
    {
<<<<<<< HEAD
        if (gameState != "playing") return; // �������� �ƴϸ� ���� �ȵǰ�
       

        rb.velocity = new Vector2(axisH, axisV) * speed; // �̵�
    }
=======
        if (gameState != "playing") return;
        if (inDamage)
        {
            //데미지 받는 중이면 깜빡깜빡 이게 만들기
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            else
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            return;  //데미지 받고 있을 때 이동 안되게 

        }

        rb.velocity = new Vector2(axisH, axisV) * speed;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")  //몬스터와 접촉 -공격받음
        {
            GetDamage(collision.gameObject);
            //inDamage = true;

        }
    }

    void GetDamage(GameObject Enemy)
    {
        if (gameState == "playing")
        {
            hp--;
            PlayerPrefs.SetInt("PlayerHP", hp);

        }
    }



    float GetAngle(Vector2 fromPt, Vector2 toPt) // 각도 구하기
    {
        float angle;
        if (axisH != 0 || axisV != 0)
        {
            float dx = toPt.x - fromPt.x;
            float dy = toPt.y - fromPt.y;
            float rad = Mathf.Atan2(dy, dx);

            angle = rad * Mathf.Rad2Deg;

        }

        else angle = angleZ;
        return angle;
    }

>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
}
