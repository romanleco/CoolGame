using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    private bool _playerHere;
    [SerializeField] private string _sceneNameToTeleportTo;
    private bool _active;
    [SerializeField] private bool _alwaysActive;
    [SerializeField] private bool _savesOnTeleport;
    [SerializeField] private AudioClip _teleportSound;
    private WaitForSeconds _teleportTime = new WaitForSeconds(1f);

    void Start()
    {
        if(_alwaysActive)
        {
            _active = true;
        }
    }
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
            if(Input.GetKeyDown(KeyCode.E) && _active)
            {
                if(_savesOnTeleport)
                {
                    SaveManager.Instance.Save();
                }
                StartCoroutine("Teleport");
            }
        }
    }

    IEnumerator Teleport()
    {
        MusicManager.Instance.fXPlayer.PlayOneShot(_teleportSound, SaveManager.Instance.fXVolume);
        GameObject.Find("Player").SetActive(false);
        yield return _teleportTime;
        SceneManager.Instance.ChangeScene(_sceneNameToTeleportTo);
    }

    public void SetFunctional()
    {
        _active = true;
    }
}
