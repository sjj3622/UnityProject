using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    public float speed = 3.0f; // 이동 속도

    float axisH, axisV;
    public static string gameState = "playing";

    //SPUM 에셋의  애니메이션 제어용
    SPUM_Prefabs spum;
    Vector2 lastMoveDir = Vector2.down;  // 마지막 바로본 방향 표시
    const float moveDeadZone = 0.05f; // 미세움직임 무시
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //gameState = "playing";
    }

    // Update is called once per frame
    void Update()
    {

        //Debug.Log("gameState ::::::" + gameState );

        if (gameState == "Battle")
        {
            currentState = PlayerState.IDLE; // 움직임 막기
            return;
        }


        if (gameState != "playing") return; 
                                                       
      
         axisH = Input.GetAxisRaw("Horizontal");  // 좌우 이동키
         axisV = Input.GetAxisRaw("Vertical"); // 상하 이동키


        if (moveDeadZone < Mathf.Abs(axisH))
        {
            //좌우 이동 방향
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

    }

    private void FixedUpdate()
    {
        if (gameState != "playing") return; // 게임중이 아니면 실행 안되게
       

        rb.velocity = new Vector2(axisH, axisV) * speed; // 이동
    }
}
