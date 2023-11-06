using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LodingSceneController.LoadScene("LowPolyFPS_Lite_Demo");
        }
    }
}
