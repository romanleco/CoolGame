using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private bool _playerHere;
    [SerializeField] private string _sceneNameToTeleportTo;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _playerHere = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            _playerHere = false;
        }
    }

    void Update()
    {
        if(_playerHere)
        {
            if(Input.GetKeyDown(KeyCode.E))
            {
                SceneManager.Instance.ChangeScene(_sceneNameToTeleportTo);
            }
        }
    }
}
