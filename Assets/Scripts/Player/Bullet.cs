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
        
        if(this.transform.parent != null)
        {
            this.transform.parent = null;
        }
    }

    void Update()
    {
        _movementVector.x = _speed * Time.deltaTime;
        transform.Translate(_movementVector);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Enemy")
        {
            IDamagable hit = other.GetComponent<IDamagable>();
            if(hit != null)
            {
                hit.ReceiveDamage(_damage);
                Destroy(this.gameObject);
            }
        }
        else if(other.tag == "Floor")
        {
            Destroy(this.gameObject);
        }
    }
}
