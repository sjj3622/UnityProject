using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{

    public float shootSpeed = 12.0f;  //화살 속도
    public float shootDelay = 0.25f;  //발사 간격

    public GameObject bowFab; // 활 프리펩
    public GameObject arrowFab; //화살 프리펩

    bool isAttack = false; //공격 중인가? 아닌가?
    GameObject bowObj; // 활 객체






    void Start()
    {
        Vector3 pos = transform.position;  //캐릭터 좌표를 활에 적용해줘야 캐릭터가 가지고 있을수 있음
        bowObj = Instantiate(bowFab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform); // 활 객체는 캐릭터의 자식으로 들어감
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire1")) //공격 키 눌렸다
        {
            Attck();
        }



        float bowZ = -1; // 활의 z값 캐릭터 보다 앞으로
        PlayerController pc = GetComponent<PlayerController>();
        if (pc.angleZ > 30 && pc.angleZ < 150)
            bowZ = 1; //위쪽 방향 이동 하는 경우


        // 활 회전
        bowObj.transform.rotation = Quaternion.Euler(0, 0, pc.angleZ);

        // 활의 화면 표시 우선순위 z축
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);

        
    }


    public void Attck()
    {
        //화살의 수량이 0 이 아닌 경우와 이미 화살이 발사된 경우가 아니라면 공격가능
        if (ItemKeeper.hasArrows > 0 && !isAttack )
        {
            ItemKeeper.hasArrows--; //화살 수량 -1
            isAttack = true; // 공격중

            PlayerController pc = GetComponent<PlayerController>();
            float angleZ = pc.angleZ;   // 캐릭터가 바라보고 있는 방향(각도)

            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowFab, transform.position, r);

            //화살이 발사될 좌표
            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);

            Vector3 v = new Vector3(x, y) * shootSpeed;

            Rigidbody2D rb = arrowObj.GetComponent<Rigidbody2D>();
            rb.AddForce(v, ForceMode2D.Impulse);

            Invoke("StopAttck", shootDelay); // 화살 재발사 시간 뒤에 공격중이 아님으로 설정

        }

    }

    public void StopAttck()
    {
        isAttack = false; // 공격중이 아니다.
    }
}
