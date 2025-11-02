using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public void StartClick() // 게임 스타트 버튼
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void ExitClick() // 게임 종료 버튼
    {
        Application.Quit(); // 유니티 게임 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 유니티 에디터의 시작단추부분
#endif
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
