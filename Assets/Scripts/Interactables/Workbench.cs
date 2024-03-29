using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private bool _playerHere;
    private bool _menuOpen;
    [SerializeField] private AudioClip _openMenuSFX;
    [SerializeField] private AudioClip _closeMenuSFX;
    [SerializeField] private GameObject _uiWorkbenchMenu;
    [SerializeField] private GameObject _interactableSign;
    [SerializeField] private GameObject _pauseMenu;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            _playerHere = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            _playerHere = false;
            if(_menuOpen)
            {
                _uiWorkbenchMenu.SetActive(false);
                _menuOpen = false;
                GameManager.Instance.SetIsOnMenu(false);
            }
            _interactableSign.SetActive(false);
        }
    }

    void Update()
    {
        if(_playerHere)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                if(_pauseMenu.activeSelf == false)
                {
                    if(_menuOpen == false)
                    {
                        _uiWorkbenchMenu.SetActive(true);
                        _menuOpen = true;
                        MusicManager.Instance.fXPlayer.PlayOneShot(_openMenuSFX, SaveManager.Instance.fXVolume);
                        _interactableSign.SetActive(false);
                        GameManager.Instance.SetIsOnMenu(true);
                    }
                    else if(_menuOpen == true)
                    {
                        CloseMenu();
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(_menuOpen == true)
                {
                    CloseMenu();
                }
            }

            if(_pauseMenu.activeSelf == false)
            {
                if(_interactableSign.activeSelf == false && _menuOpen == false)
                {
                    _interactableSign.SetActive(true);
                }
            }
            else
            {
                _interactableSign.SetActive(false);
            }
        }
    }

    private void CloseMenu()
    {
        _uiWorkbenchMenu.SetActive(false);
        _menuOpen = false;
        MusicManager.Instance.fXPlayer.PlayOneShot(_closeMenuSFX, SaveManager.Instance.fXVolume);
        GameManager.Instance.SetIsOnMenu(false);
    }
}
