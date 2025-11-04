using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public float smooth = 8f;




    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Lerp(pos.x, target.position.x, Time.deltaTime * smooth);
        pos.y = Mathf.Lerp(pos.y, target.position.y, Time.deltaTime * smooth);
        transform.position = pos;
    }




    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
