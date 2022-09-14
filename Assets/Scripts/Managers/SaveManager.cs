using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("SaveManager the SaveManager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }
}
