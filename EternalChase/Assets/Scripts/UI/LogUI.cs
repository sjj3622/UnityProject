using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour
{
    [SerializeField] Text logText; 

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void Print(string msg)
    {
        logText.text = msg;
    }
}
