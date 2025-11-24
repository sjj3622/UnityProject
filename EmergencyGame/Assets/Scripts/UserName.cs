using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserName : MonoBehaviour
{
    void Update()
    {
        // 실행 시 전달된 명령어 인자 가져오기
        string[] args = System.Environment.GetCommandLineArgs();

        // args[0]은 실행 파일 경로
        string userId = args.Length > 1 ? args[1] : "guest";
        string Name = args.Length > 2 ? args[2] : "게스트";

        Debug.Log("UserId: " + userId);
        Debug.Log("UserName: " + Name);

        // TextMeshPro 등 텍스트 오브젝트에 적용
        var textObj = GameObject.Find("PlayerName"); // 씬에 있는 Text 오브젝트 이름
        if (textObj != null)
        {
            var textComponent = textObj.GetComponent<UnityEngine.UI.Text>();
            if (textComponent != null)
            {
                textComponent.text = Name + " 님";
            }
        }
    }
}