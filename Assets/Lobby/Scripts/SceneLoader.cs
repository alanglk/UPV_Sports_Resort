using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneOnTop(Scene scene)
    {
        SceneManager.LoadScene(scene.ToString(), LoadSceneMode.Additive);
    }

    public void UnLoadSceneOnTop(Scene scene)
    {
        int n = SceneManager.sceneCount;
        if (n > 1)
        {
            SceneManager.UnloadSceneAsync(scene.ToString());
        }
    }
}