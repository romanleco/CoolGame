using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManager : MonoBehaviour
{
    private static SceneManager _instance;
    public static SceneManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("SceneManager the SceneManager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public void ChangeScene(string SceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
