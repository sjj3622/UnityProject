using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    private BDSceneStateManager sceneStateManager;

    void Start()
    {
        legacyText = textObject.GetComponent<LegacyText>();
        sceneStateManager = FindAnyObjectByType<BDSceneStateManager>();
    }

    public void OPBtn1Click()
    {
        if (OpBtn1 != null)
        {
            // 텍스트 변경
            legacyText.myText.text = "일단 피를 닦아보자";

            BDgpManager.gameState = "BDReady";
            OpBtn1.SetActive(false);
            OpBtn2.SetActive(false);

            // 3초 뒤 씬 전환 코루틴 실행
            StartCoroutine(LoadNextSceneAfterDelay(3f, "BleedingGamepalying"));
        }
    }

    public void OPBtn2Click()
    {
        legacyText.myText.text = "당신은 사람을 살릴 자격이 없습니다.";

        OpBtn1.SetActive(false);
        OpBtn2.SetActive(false);

        StartCoroutine(Exit(3f, "Title"));
    }




    private IEnumerator LoadNextSceneAfterDelay(float delay, string sceneName)
    {
        yield return new WaitForSeconds(delay); // delay 초 기다림

        BDSceneStateManager.instance.SaveState(GameObject.Find("Patient"));
        BDSceneStateManager.instance.SaveState(GameObject.Find("Player"));
        //BDSceneStateManager.instance.SaveState(GameObject.Find("Timer"));
        BDSceneStateManager.instance.SaveState(Camera.main.gameObject);

        SceneManager.LoadScene(sceneName);       // 씬 전환
    }

    private IEnumerator Exit(float delay, string sceneName)
    {
        
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(sceneName);
    }

    public void Again()
    {
        Debug.Log("클릭");
        BDgpManager.gameState = "";

        if (BDSceneStateManager.instance != null)
        {
            BDSceneStateManager.instance.ClearSaved();
        }
        else
        {
            Debug.LogError("BDSceneStateManager.instance가 null입니다!");
        }

        Debug.Log("씬 이름: " + SceneManager.GetActiveScene().name);
        Debug.Log("BDSceneStateManager.instance: " + BDSceneStateManager.instance);

        SceneManager.LoadScene("Bleeding");
    }

    public void Exit()
    {
        Debug.Log("클릭");
        SceneManager.LoadScene("Title");
    }
}