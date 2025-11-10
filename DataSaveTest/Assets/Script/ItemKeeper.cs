using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class ItemKeeper : MonoBehaviour
{
    public static int hasKeys = 0; // 열쇠 수량
    public static int hasArrows = 0;//화살 수량


    void Start()
    {
        hasKeys = PlayerPrefs.GetInt("Keys");
        //hasArrows = PlayerPrefs.GetInt("Arrows");
        StartCoroutine(GetArrow());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void SaveItem()
    {
        // 아이템 저장하기
        PlayerPrefs.SetInt("Keys", hasKeys);
        PlayerPrefs.SetInt("Arrows", hasArrows);
    }

    IEnumerator GetArrow()  // 웹서버로 부터 화살 수량 받아오기
    {
        string url = "http://localhost:8080/api/getarrow";


        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.timeout = 10; // 응답대기 최대 시간

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string json = request.downloadHandler.text;
                ArrowData arrowData = JsonUtility.FromJson<ArrowData>(json);

                ItemKeeper.hasArrows = arrowData.count;

                Debug.Log("성공 : " + ItemKeeper.hasArrows);
            }
            else
            {
                Debug.Log("실패 : " + request.error);
            }
        }
    }
}