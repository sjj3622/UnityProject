using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class userIDtest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] args = System.Environment.GetCommandLineArgs();
        if (args.Length > 1)
        {
            string userId = args[1];
            Debug.Log("서버에서 전달받은 userId: " + userId);
        }
        else
        {
            Debug.Log("userId가 전달되지 않았습니다.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
