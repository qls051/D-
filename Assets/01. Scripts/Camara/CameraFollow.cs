using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform targetTransform;
 
    void Update()
    {
        transform.position = targetTransform.position;        
    }
}
