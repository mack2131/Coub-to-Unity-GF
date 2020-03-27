using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
        else target = GameObject.FindObjectOfType<PlayerController>().transform;        
    }
}
