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

    void Start()
    {
        _spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Weapon::Start() SpriteRenderer _spriteRenderer is null");
        }
    }

    void Update() 
    {
        if(Input.GetMouseButtonDown(0))
        {
            Instantiate(_bulletPrefab, _bulletSpawn.transform.position, transform.rotation);
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
        }
        else
        {
            _spriteRenderer.flipY = false;
        }
    }
}
