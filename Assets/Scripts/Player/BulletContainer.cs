using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletContainer : MonoBehaviour
{
    [SerializeField] private float _lifeTime = 0.1f;
    void Start()
    {
        Destroy(this.gameObject, _lifeTime);
    }
}
