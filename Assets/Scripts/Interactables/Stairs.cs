using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    private bool _playerHere;
    [SerializeField] private bool _up;
    private bool _active;
    private Vector2 _teleportVector;
    private float _teleportDistance = 19.5f;
    private GameObject _player;
    [SerializeField] private SpriteRenderer _sprRenderer;
    [SerializeField] private Sprite _blackStairSprite;
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            if(_player == null)
            {
                _player = other.gameObject;
            }
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
                _teleportVector = _player.transform.position;
                if(_up)
                {
                    _teleportVector.y += _teleportDistance;
                }
                else
                {
                    _teleportVector.y -= _teleportDistance;
                }
                _player.transform.position = _teleportVector;
            }
        }
    }

    public void SetFunctional(bool functional)
    {
        _sprRenderer.sprite = _blackStairSprite;
        _active = functional;
    }
}
