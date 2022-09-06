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

    void Start()
    {
        _fireTimer = new WaitForSeconds(_fireRate);
        _localPosition.y = -0.108f;
    }

    void Update() 
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

        PointAtMouse();
        SideFlip();
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
            transform.localScale = new Vector2(1, -1);
            _localPosition.x = -_xOffset;
        }
        else if(difference.x < -0.35f)
        {
            transform.localScale = new Vector2(1, 1);
            _localPosition.x = _xOffset;
        }
        transform.localPosition = _localPosition;
    }

    IEnumerator FireRoutine()
    {
        Instantiate(_bulletPrefab, _bulletSpawn.transform.position, transform.rotation);
        _canFire = false;
        yield return _fireTimer;
        _canFire = true;
    }
}
