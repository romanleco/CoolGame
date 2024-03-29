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

        Cursor.SetCursor(_cursorTex, new Vector2(10, 10), CursorMode.ForceSoftware);
    }

    void Start()
    {
        string currentSceneName = SceneManager.Instance.GetSceneName();
        switch(currentSceneName)
        {
            case "BaseScene":
                MusicManager.Instance.PlayBase();
            break;

            case "MainScene":
                MusicManager.Instance.PlayLoopable();
            break;

            case "MainMenuScene":
                MusicManager.Instance.PlayMainMenu();
                if(SaveManager.Instance.Load() == null)
                {
                    GameObject.Find("ContinueButton").SetActive(false);
                }
            break;

            default:
                Debug.LogError("GameManager.Start() SceneName not found");
            break;
        }
    }

    [SerializeField] public int metalPlates{get; private set;} //* id = 0
    [SerializeField] public int energyCores{get; private set;} //* id = 1
    [SerializeField] public int gears{get; private set;} //* id = 2
    [SerializeField] public int circuitBoards{get; private set;} //* id = 3
    [SerializeField] private Texture2D _cursorTex;

    public Vector2 playerPosition{ get; private set;}
    public bool isGamePaused{get; private set;}
    public bool isOnMenu{get; private set;}
    public IDamagable playerDamagableInterface{get; private set;}
    public int currentWeaponIndex{get; private set;}

    public void UpdatePlayerPosition(Vector2 newPosition)
    {
        playerPosition = newPosition;
    }

    public void SetPlayerDamagableInterface(IDamagable iDamagable)
    {
        playerDamagableInterface = iDamagable;
    }

    public void SetGameState(bool gamePaused)
    {
        isGamePaused = gamePaused;
    }

    public void SetIsOnMenu(bool onMenu)
    {
        isOnMenu = onMenu;
    }

    public void SetCurrentWeaponIndex(int index)
    {
        currentWeaponIndex = index;
    }

    public void AddResource(int resourceId)
    {
        switch(resourceId)
        {
            case 0:
                metalPlates++;
            break;

            case 1:
                energyCores++;
            break;

            case 2:
                gears++;
            break;

            case 3:
                circuitBoards++;
            break;

            default:
                Debug.LogError("GameManager::AddResource case not found");
            break;
        }
        //update ui
    }

}
