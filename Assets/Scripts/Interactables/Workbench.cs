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
                if(_menuOpen == false)
                {
                    _uiWorkbenchMenu.SetActive(true);
                    _menuOpen = true;
                    AudioSource.PlayClipAtPoint(_openMenuSFX, Vector2.zero, 1f);
                    _interactableSign.SetActive(false);
                    GameManager.Instance.SetIsOnMenu(true);
                }
                else if(_menuOpen == true)
                {
                    _uiWorkbenchMenu.SetActive(false);
                    _menuOpen = false;
                    AudioSource.PlayClipAtPoint(_closeMenuSFX, Vector2.zero, 1f);
                    GameManager.Instance.SetIsOnMenu(false);
                }
            }

            if(_interactableSign.activeSelf == false && _menuOpen == false)
            {
                _interactableSign.SetActive(true);
            }
        }
    }
}
