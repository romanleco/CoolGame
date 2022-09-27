using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workbench : MonoBehaviour
{
    private bool _playerHere;
    private bool _menuOpen;
    [SerializeField] private GameObject _uiWorkbenchOneMenu;
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
                }
                else if(_menuOpen == true)
                {
                    _uiWorkbenchOneMenu.SetActive(false);
                    _menuOpen = false;
                }
            }
        }
    }
}
