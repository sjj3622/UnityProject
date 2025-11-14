using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BDPanel : MonoBehaviour
{
    public Text ClearCount; // Text 컴포넌트를 연결
    BDSceneStateManager BDSceneStateManager;

    void Start()
    {
        BDSceneStateManager = FindAnyObjectByType<BDSceneStateManager>();
    }

    public void AgainClick()
    {
        
        BDgpManager.gameState = "";
        BDSceneStateManager.ClearSaved();
        SceneManager.LoadScene("Bleeding");

    }

    public void ExitClick()
    {
        
        SceneManager.LoadScene("Title");
    }

    public void CountDown()
    {
        Debug.Log("카운트 다운시작");
        StartCoroutine(CountdownAndLoadScene());
    }

    IEnumerator CountdownAndLoadScene()
    {
        int countdown = 3;

        while (countdown > 0)
        {
            ClearCount.text = countdown + "초 뒤 돌아갑니다";
            yield return new WaitForSeconds(1f); // 1초 대기
            countdown--;
        }

        // 마지막 0초 표시 (선택 사항)
        ClearCount.text = "돌아갑니다!";
        yield return new WaitForSeconds(1f);

    }
}
