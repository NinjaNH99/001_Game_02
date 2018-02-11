using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoSingleton<LevelLoader>
{
    public void LoadLevel(string SceneIndex)
    {
        StartCoroutine(LoadAsynch(SceneIndex));
    }

    IEnumerator LoadAsynch(string sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while(!operation.isDone)
        {
            //Debug.Log(operation.progress);

            yield return null;
        }
    }
}
