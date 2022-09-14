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

    [SerializeField] public int metalPlates{get; private set;} //* id = 0
    [SerializeField] public int energyCores{get; private set;} //* id = 1
    [SerializeField] public int gears{get; private set;} //* id = 2
    [SerializeField] public int circuitBoards{get; private set;} //* id = 3

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

    public void AddResource(int resourceId)
    {
        switch(resourceId)
        {
            case 0:
                metalPlates++;
                Debug.Log("Metal Plates: " + metalPlates);
            break;

            case 1:
                energyCores++;
                Debug.Log("Energy Cores: " + energyCores);
            break;

            case 2:
                gears++;
                Debug.Log("Gears: " + gears);
            break;

            case 3:
                circuitBoards++;
                Debug.Log("Circuit Boards: " + circuitBoards);
            break;

            default:
                Debug.LogError("GameManager::AddResource case not found");
            break;
        }
        //update ui
    }

}
