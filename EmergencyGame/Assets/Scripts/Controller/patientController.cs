using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class patientController : MonoBehaviour
{
    public Transform tr;


    



    void Start()
    {
        this.tr = transform;
        tr.Translate(new Vector3(-3, 0, 0));
    }



    void Update()
    {
        
    }
}
