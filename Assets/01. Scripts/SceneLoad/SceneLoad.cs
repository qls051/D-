using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void SceneLoadButton()
    {
        LoadingSceneController.LoadScene("GameMenu");
    }
}
