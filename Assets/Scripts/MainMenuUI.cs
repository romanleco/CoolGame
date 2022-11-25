using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject _settingsMenu;
    [SerializeField] private GameObject _creditsMenu;
    [SerializeField] private TMP_Text _musicVolumeValueText;
    [SerializeField] private TMP_Text _sFXVolumeValueText;

    void Start()
    {
        DataContainer loadedData = SaveManager.Instance.Load();
        if(loadedData != null)
        {
            _musicVolumeValueText.text = loadedData.volume.ToString();
            _sFXVolumeValueText.text = loadedData.fXVolume.ToString();
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            CloseMenu(0);
            CloseMenu(1);
        }
    }


    public void Continue()
    {
        SceneManager.Instance.ChangeScene("BaseScene");
    }

    public void NewGame()
    {
        SaveManager.Instance.Save(0, 0, 0, 0);
        SaveManager.Instance.UnlockUpgrade(1, 1, false);
        SaveManager.Instance.UnlockUpgrade(1, 2, false);
        SaveManager.Instance.UnlockUpgrade(1, 3, false);

        SaveManager.Instance.UnlockWeapon(0);
        SaveManager.Instance.UnlockWeapon(1, false);
        SaveManager.Instance.UnlockWeapon(2, false);
        SaveManager.Instance.UnlockWeapon(3, false);

        SceneManager.Instance.ChangeScene("BaseScene");
    }

    public void Settings()
    {
        _settingsMenu.SetActive(true);
        DataContainer data = SaveManager.Instance.Load();
        _musicVolumeValueText.text = data.volume.ToString();
        _sFXVolumeValueText.text = data.fXVolume.ToString();
    }

    public void Credits()
    {
        _creditsMenu.SetActive(true);
    }

    public void CloseMenu(int menuID)
    {
        switch(menuID)
        {
            case 0:
                _settingsMenu.SetActive(false);
            break;

            case 1:
                _creditsMenu.SetActive(false);
            break;

            default:
                Debug.LogError("CloseMenu menuID not found");
            break;
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.Instance.ChangeScene("MainMenuScene");
    }

    public void MusicVolume(bool isDown)
    {
        DataContainer data = SaveManager.Instance.Load();
        if(isDown)
        {
            if((data.volume <= 0) == false)
            {
                MusicManager.Instance.AdjustVolume(data.volume - 5);
                _musicVolumeValueText.text = (data.volume - 5).ToString();
            }
        }
        else
        {
            if((data.volume + 5) <= 100)
            {
                MusicManager.Instance.AdjustVolume(data.volume + 5);
                _musicVolumeValueText.text = (data.volume + 5).ToString();
            }
        }
    }

    public void SFXVolume(bool isDown)
    {
        DataContainer data = SaveManager.Instance.Load();
        if(isDown)
        {
            if((data.fXVolume <= 0) == false)
            {
                SaveManager.Instance.AdjustVolume(data.fXVolume - 5, false);
                _sFXVolumeValueText.text = (data.fXVolume - 5).ToString();
            }
        }
        else
        {
            if((data.fXVolume + 5) <= 100)
            {
                SaveManager.Instance.AdjustVolume(data.fXVolume + 5, false);
                _sFXVolumeValueText.text = (data.fXVolume + 5).ToString();
            }
        }
    }

}
