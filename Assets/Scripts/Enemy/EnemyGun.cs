using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private Vector2 _aimingVector;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _barrel;
    private WaitForSeconds _cooldownTimer;
    void Start()
    {
        _cooldownTimer = new WaitForSeconds(Random.Range(1, 3));
    }

    void Update()
    {
        PointAtPlayer();
    }

    private void PointAtPlayer()
    {
        _aimingVector.x = (GameManager.Instance.playerPosition.x - transform.position.x);
        _aimingVector.y = (GameManager.Instance.playerPosition.y - transform.position.y);
        float angle = Mathf.Atan2(_aimingVector.y, _aimingVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void Shoot()
    {
        Instantiate(_projectile, _barrel.position, Quaternion.identity);
    }

    IEnumerator Shooting()
    {
        Shoot();
        yield return _cooldownTimer;
        StartCoroutine("Shooting");
    }
}
