using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private GameObject _bulletSpawn;
    private Vector3 _mousePos;
    [SerializeField] private Camera _playerPointerCam;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Vector2 _instantiationPoint;
    [SerializeField] float _correctionOffset;
    [SerializeField] private bool _isAutomatic;
    private bool _isFiring;
    [SerializeField] private bool _canFire = true;
    private WaitForSeconds _fireTimer;
    [SerializeField] private float _fireRate = 0.1f;

    void Start()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Weapon::Start() SpriteRenderer _spriteRenderer is null");
        }
        _fireTimer = new WaitForSeconds(_fireRate);
    }

    void Update() 
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
        if(difference.x > 0)
        {
            _spriteRenderer.flipY = true;
            _instantiationPoint = new Vector2(_bulletSpawn.transform.position.x, _bulletSpawn.transform.position.y + _correctionOffset);
        }
        else
        {
            _spriteRenderer.flipY = false;
            _instantiationPoint = _bulletSpawn.transform.position;
        }
    }

    IEnumerator FireRoutine()
    {
        Instantiate(_bulletPrefab, _instantiationPoint, transform.rotation);
        _canFire = false;
        yield return _fireTimer;
        _canFire = true;
    }
}
