using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_Boom_Effect : MonoBehaviour
{
    public float destroyTime = 1.5f;
    float currentTime = 0;

    void Update()
    {
        if(currentTime > destroyTime)
        {
            Destroy(gameObject);
        }
        currentTime += Time.deltaTime;
    }
}
