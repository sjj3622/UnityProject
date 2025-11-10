using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public enum ItemType
{
    Arrow, Key, Life,
}



public class ItemData : MonoBehaviour
{
    public ItemType Type;
    public int count = 1;
    public int arrangeId = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //if (Type == ItemType.Arrow) ItemKeeper.hasArrows += count;
            switch (Type)
            {
                case ItemType.Arrow:
                    ItemKeeper.hasArrows += count;

                    // yield 를 통해 비동기 작업을 수행하는데  
                    // 어느정도의 시간이 걸리는지 알수 없기에 응답이 올때까지 지속될수있게
                    StartCoroutine(SendArrowCount(ItemKeeper.hasArrows));

                    break;
                case ItemType.Key:
                    ItemKeeper.hasKeys += count; break;
                case ItemType.Life:
                    PlayerController.hp++;
                    PlayerPrefs.SetInt("PlayerHP", PlayerController.hp);
                    break;
            }

            gameObject.GetComponent<CircleCollider2D>().enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            rb.gravityScale = 2.5f;
            rb.AddForce(new Vector2(0, 6), ForceMode2D.Impulse);
            Destroy(gameObject, 0.5f);
            // SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);
        }
    }


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator SendArrowCount(int count)
    {
        // 유니티 게임 데이터를 서버에 보내기 
        // 1. 서버의 주소
        // 2. json 데이터 확보
        // 3. 전송방식
        // 4. Header Set
        // 5. 전송

        string url = "http://localhost:8080/api/datatest";

        string data = JsonUtility.ToJson(new ArrowData(count));

        UnityWebRequest request = new UnityWebRequest(url, "POST");

        //문자열 json을 UTF-8 바이트로 변환 - HTTP는 바이트여야하므로 필수
        byte[] body = Encoding.UTF8.GetBytes(data);

        //변환한 바이트를 요청 본문으로 설정,  UpLoadHandlerRaw는 바이트를 전송
        request.uploadHandler = new UploadHandlerRaw(body);

        // 서버 응답을 메모리에 버퍼링 해서 읽기 쉽게하기
        request.downloadHandler = new DownloadHandlerBuffer();

        //요청 헤더에 컨텐츠 타입 명시 ( 보내는 내용은 json이야  라고 서버가 알수 있게)
        request.SetRequestHeader("Content-Type", "application/json");

        // 비동기 전송 시작
        // 서버에 응답이 올때까지 대기
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("성공 : " + request.downloadHandler.text);
        }
        else
        {
            Debug.Log("실패 : " + request.error);
        }
    }

    

}
[System.Serializable]  // 데이터 저장용 클래스 작성시
    public class ArrowData
    {
        public string item = "arrow";
        public int count;

        public ArrowData(int count) { this.count = count; }
    }