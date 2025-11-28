using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerCount : MonoBehaviour
{
    public static TimerCount Instance;
    public int timerCount = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // ✅ 이미 존재하면 제거
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ✅ 1개만 유지
    }

    void Start()
    {
        timerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
