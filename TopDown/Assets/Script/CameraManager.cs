using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{

    public GameObject otherTarget;



    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 카메라 조작하기 - 보스를 만난경우 카메라위치 때문에 몬스터가 안보임
        // 보스와의 거리를 이용하거나 보스존을 만들어서 영역안에 들어가면 동작하도록 한다



        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (otherTarget != null) // 보스가 있는 맵이라면
        {
            float dist = Vector2.Distance(otherTarget.transform.position,
                player.transform.position);
            if(dist < 8.0f)
            {
                //Camera.main.orthographicSize = 8;
                //카메라 거리 변경을 부드럽게 처리하려면
                Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,
                    8.0f, Time.deltaTime * 2f);
            }
        }
        
            //보스 몬스터가 없는 맵에서 카메라 동작
            transform.position = new Vector3(player.transform.position.x,
                player.transform.position.y, -10);
        


    }
}
