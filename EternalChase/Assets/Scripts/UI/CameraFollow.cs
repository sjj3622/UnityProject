<<<<<<< HEAD
using System.Collections;
=======
ï»¿using System.Collections;
>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
<<<<<<< HEAD
    public Transform target; // Ä³¸¯ÅÍ
    public float smooth = 8f;

=======

    public Transform target;
    public float smooth = 8f;




>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, Time.deltaTime * smooth);
        pos.y = Mathf.Lerp(pos.y, target.position.y, Time.deltaTime * smooth);
        transform.position = pos;
    }

<<<<<<< HEAD
=======



>>>>>>> 22ff9ed88032a66f3cc0fc3a068ec87347d487ea
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
