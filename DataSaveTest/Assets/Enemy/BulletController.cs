using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    public float deleteTime = 3.0f;




    void Start()
    {
        Destroy(gameObject,deleteTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    

    




    void Update()
    {
        
    }
}
