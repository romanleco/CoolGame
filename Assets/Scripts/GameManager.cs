using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.LogError("GameManager the Game Manager is null");
            }
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    public Vector2 playerPosition{ get; private set;}
    public IDamagable playerDamagableInterface{get; private set;}

    public void UpdatePlayerPosition(Vector2 newPosition)
    {
        playerPosition = newPosition;
    }

    public void SetPlayerDamagableInterface(IDamagable iDamagable)
    {
        playerDamagableInterface = iDamagable;
    }
}
