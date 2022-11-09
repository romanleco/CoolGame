using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private bool _playerHere;
    private bool _menuOpen;
    [SerializeField] private AudioClip _openMenuSFX;
    [SerializeField] private AudioClip _closeMenuSFX;
    [SerializeField] private GameObject _uiWorkbenchOneMenu;
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
                _uiWorkbenchOneMenu.SetActive(false);
                _menuOpen = false;
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
                    _uiWorkbenchOneMenu.SetActive(true);
                    _menuOpen = true;
                    AudioSource.PlayClipAtPoint(_openMenuSFX, Vector2.zero, 1f);
                    _interactableSign.SetActive(false);
                }
                else if(_menuOpen == true)
                {
                    _uiWorkbenchOneMenu.SetActive(false);
                    _menuOpen = false;
                    AudioSource.PlayClipAtPoint(_closeMenuSFX, Vector2.zero, 1f);
                }
            }

            if(_interactableSign.activeSelf == false && _menuOpen == false)
            {
                _interactableSign.SetActive(true);
            }
        }
    }
}
