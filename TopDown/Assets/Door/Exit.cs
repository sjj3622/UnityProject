using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ExitDirection
{
    Left,
    Right,
    Up,
    Down
}

public class Exit : MonoBehaviour
{
    public string sceneName = "";
    public int doorNum = 0;
    public ExitDirection direction = ExitDirection.Down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(doorNum == 100)  //보스 잡고 문에 도달한 경우
                GameObject.FindObjectOfType<UIManager>().gameClear();
            
            else RoomManager.LoadScene(sceneName, doorNum);

        }
    }





    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
