using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burncamera : MonoBehaviour
{
    public Transform player; 
    public float followSpeed = 5f;
    private Camera cam;
    

    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = 10f; 
        Debug.Log("Camera size set to 10");
    }

   
    void Update()
    {
        
    }
}
