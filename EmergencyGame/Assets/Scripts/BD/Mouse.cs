using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse : MonoBehaviour
{
    public Texture2D[] cursorImages;

   

    public int mouseImg = 0;
    

    private BleedController bleedController;
    private BDScoreController scoreController;
    private Blood Blood;


    void Start()
    {
        // 시작 시 첫 번째 이미지로 커서 설정
        //Cursor.SetCursor(cursorImage1, new Vector2(cursorImage1.width / 2, cursorImage1.height / 2), CursorMode.Auto);
        Cursor.SetCursor(cursorImages[0], new Vector2(cursorImages[0].width / 2, cursorImages[0].height / 2), CursorMode.Auto);
        bleedController = FindAnyObjectByType<BleedController>();

        scoreController = FindAnyObjectByType<BDScoreController>();

        Blood = FindAnyObjectByType<Blood>();
    }


    void Update()
    {
        // 마우스 위치를 실시간으로 확인 (Debug 용)
        Vector3 mousePos = Input.mousePosition;
        //Debug.Log("Mouse Position: " + mousePos);

        SpaceDown();

    }

    public void ResetToDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // 기본 커서로 변경
        mouseImg = 0; // 선택된 커서 인덱스도 초기화
        Debug.Log("커서를 기본 OS 커서로 초기화");
    }

    private void OnDisable()
    {
        ResetToDefaultCursor();
    }
    void SpaceDown()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // mouseImg 증가
            mouseImg++;

            // 배열 범위를 벗어나면 0으로 초기화
            if (mouseImg >= cursorImages.Length)
                mouseImg = 0;

            // 현재 mouseImg에 맞는 커서 설정
            Texture2D currentCursor = cursorImages[mouseImg];
            Cursor.SetCursor(currentCursor, new Vector2(currentCursor.width / 2, currentCursor.height / 2), CursorMode.Auto);

            Debug.Log("커서 이미지 변경: " + mouseImg);
        }
    }


    //public void RemoveClickedBleed()
    //{

    //    switch (mouseImg)
    //    {
    //        case 0: targetBloodName = "blood1"; break;
    //        case 1: targetBloodName = "blood2"; break;
    //        case 2: targetBloodName = "blood3"; break;
    //        case 3: targetBloodName = "blood4"; break;
    //    }


    //    if (bleedController == null) return;

    //    // 마우스 클릭 위치 (월드 좌표)
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    mousePos.z = 0f; // 2D 기준

    //    // targetSprite 하위 모든 피 오브젝트 확인
    //    for (int i = bleedController.targetSprite.transform.childCount - 1; i >= 0; i--)
    //    {
    //        Transform child = bleedController.targetSprite.transform.GetChild(i);
    //        float distance = Vector2.Distance(new Vector2(mousePos.x, mousePos.y),
    //                                          new Vector2(child.position.x, child.position.y));

    //        if (distance < 1.0f && child.name == targetBloodName)
    //        {
    //            // 피 제거
    //            Destroy(child.gameObject);
    //            Debug.Log("클릭된 피 오브젝트: " + child.name);

    //            // 이 피에 붙은 Blood 스크립트 가져오기
    //            Blood blood = child.GetComponent<Blood>();
    //            if (blood != null)
    //            {

    //                blood.ScorePlus();
    //            }
    //            else
    //            {
    //                Debug.Log("Blood 스크립트 없음!");
    //            }
    //        }

    //    }

    //}

    public void RemoveClickedBleed()
    {
        //Debug.Log("함수 시작");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        Collider2D hitCollider = Physics2D.OverlapPoint(mousePos2D);

        if (hitCollider != null)
        {
            Debug.Log("클릭된 오브젝트: " + hitCollider.name);

            BloodObject bloodObj = hitCollider.GetComponent<BloodObject>();
            if (bloodObj != null)
            {
               // Debug.Log("bloodObj: " + bloodObj);
                //bloodObj.OnClicked(mouseImg);
            }
            else
            {
                Debug.Log("BloodObject 스크립트 없음!");
            }
        }
        else
        {
            Debug.Log("클릭된 오브젝트 없음!");
        }
    }
}
