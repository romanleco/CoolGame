using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private int _damage;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;
    private Vector2 _movementVector;
    void Start()
    {
        Destroy(this.gameObject, _lifetime);
    }

    void Update()
    {
        _movementVector.x = _speed * Time.deltaTime;
        transform.Translate(_movementVector);
    }
}
