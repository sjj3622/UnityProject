using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public int arrangeId = 0;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (ItemKeeper.hasKeys > 0) // 열쇠를 가지고 있냐?
            {
                ItemKeeper.hasKeys--;  //열쇠감소
                Destroy(this.gameObject);

                SaveDataManager.SetArrangeId(arrangeId, gameObject.tag);

                Debug.Log("문 번호"+arrangeId);
                Debug.Log("태그"+ gameObject.tag);
            }
        }
    }


    void Start()
    {

    }


    void Update()
    {

    }
}
