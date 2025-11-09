using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour
{
    public GameObject OpBtn1; // 보기1
    public GameObject OpBtn2; // 보기2
    public GameObject textObject; //로그 

    public Text OP1Text;
    public Text OP2Text;

    private LegacyText legacyText;

    void Start()
    {
        legacyText = textObject.GetComponent<LegacyText>();
    }

    public void OPBtn1Click()
    {
        if (OpBtn1 != null)
        {
            // 텍스트 변경
            legacyText.myText.text = "일단 피를 닦아보자";

            // 3초 뒤 씬 전환 코루틴 실행
            StartCoroutine(LoadNextSceneAfterDelay(3f, "FTGamepalying"));
        }
    }

    private IEnumerator LoadNextSceneAfterDelay(float delay, string sceneName)
    {
        yield return new WaitForSeconds(delay); // delay 초 기다림
        SceneManager.LoadScene(sceneName);       // 씬 전환
    }
}
