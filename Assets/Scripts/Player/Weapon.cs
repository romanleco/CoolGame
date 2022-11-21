using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletSpawn;
    private Vector3 _mousePos;
    [SerializeField] private Camera _playerPointerCam;
    [SerializeField] private bool _isAutomatic;
    private bool _isFiring;
    [SerializeField] private bool _canFire = true;
    private WaitForSeconds _fireTimer;
    [SerializeField] private float _fireRate = 0.1f;
    private Vector2 _localPosition;
    [SerializeField] private float _xOffset = 0.3f;
    [SerializeField] private float _yOffset = -0.108f;
    private float _startingLocalScaleX = 1;
    private float _startingLocalScaleY = 1;
    [SerializeField] private AudioClip _shotSound;
    [Header("Recoil")]
    [SerializeField] private GameObject _weapon;
    [SerializeField] private Transform _recoilPointA;
    [SerializeField] private Transform _recoilPointB;
    private float _timeElapsed;
    [SerializeField] private float _recoilDuration;
    private bool _goingBack;
    private bool _recoilActive;

    void Start()
    {
        _fireTimer = new WaitForSeconds(_fireRate);
        _localPosition.y = _yOffset;

        _startingLocalScaleX = transform.localScale.x;
        _startingLocalScaleY = transform.localScale.y;
    }

    void Update() 
    {
        if(GameManager.Instance.isGamePaused == false && GameManager.Instance.isOnMenu == false)
        {
            Firing();
            PointAtMouse();
            SideFlip();
            Recoil();
        }
    }

    private void Firing()
    {
        if(_isAutomatic)
            {
                if(Input.GetMouseButtonDown(0) || _isFiring)
                {
                    _isFiring = true;
                    if(_canFire)
                    {
                        StartCoroutine("FireRoutine");
                    }
                }

                if(Input.GetMouseButtonUp(0))
                {
                    _isFiring = false;
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))
                {
                    if(_canFire)
                    {
                        StartCoroutine("FireRoutine");
                    }
                }
            }
    }

    private void PointAtMouse()
    {
        _mousePos = _playerPointerCam.ScreenToWorldPoint(Input.mousePosition);
        Vector2 lookDir = (_mousePos - transform.position);
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SideFlip()
    {
        Vector3 difference = transform.position - _mousePos;
        if(difference.x > 0.35f)
        {
            transform.localScale = new Vector2(_startingLocalScaleX, -_startingLocalScaleY);
            _localPosition.x = -_xOffset;
        }
        else if(difference.x < -0.35f)
        {
            transform.localScale = new Vector2(_startingLocalScaleX, _startingLocalScaleY);
            _localPosition.x = _xOffset;
        }
        transform.localPosition = _localPosition;
    }

    IEnumerator FireRoutine()
    {
        Instantiate(_bulletPrefab, _bulletSpawn.transform.position, transform.rotation);
        MusicManager.Instance.fXPlayer.PlayOneShot(_shotSound, SaveManager.Instance.fXVolume);
        _canFire = false;
        _recoilActive = true;
        yield return _fireTimer;
        _canFire = true;
    }

    private void Recoil()
    {
        if(_recoilActive)
        {
            if(_goingBack)
            {
                if(_timeElapsed < _recoilDuration)
                {
                    _weapon.transform.position = Vector2.Lerp(_recoilPointB.position, _recoilPointA.position, _timeElapsed / _recoilDuration);
                    _timeElapsed += Time.deltaTime;
                }
                else
                {
                    _weapon.transform.position = _recoilPointA.position;
                    _goingBack = false;
                    _recoilActive = false;
                    _timeElapsed = 0;
                }
            }
            else
            {
                if(_timeElapsed < _recoilDuration)
                {
                    _weapon.transform.position = Vector2.Lerp(_recoilPointA.position, _recoilPointB.position, _timeElapsed / _recoilDuration);
                    _timeElapsed += Time.deltaTime;
                }
                else
                {
                    _weapon.transform.position = _recoilPointB.position;
                    _goingBack = true;
                    _timeElapsed = 0;
                }
            }
        }
    }
}
