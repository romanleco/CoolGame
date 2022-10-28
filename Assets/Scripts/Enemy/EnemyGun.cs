using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGun : MonoBehaviour
{
    private Vector3 _aimingVector;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _barrel;
    private WaitForSeconds _cooldownTimer;
    private Vector2 _localPosition;
    [SerializeField] private AudioClip _shootingSound;
    // [SerializeField] private float _xOffset;
    void Start()
    {
        _cooldownTimer = new WaitForSeconds(Random.Range(2f, 5f));
        _localPosition.y = 0.075f;
    }

    void Update()
    {
        PointAtPlayer();
        SideFlip();
    }

    private void PointAtPlayer()
    {
        _aimingVector.x = (GameManager.Instance.playerPosition.x - transform.position.x);
        _aimingVector.y = (GameManager.Instance.playerPosition.y - transform.position.y);
        float angle = Mathf.Atan2(_aimingVector.y, _aimingVector.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void SideFlip()
    {
        Vector3 difference = transform.position - _aimingVector;
        if(difference.x > 0.35f)
        {
            transform.localScale = new Vector2(1, -1);
            // _localPosition.x = -_xOffset;
        }
        else if(difference.x < -0.35f)
        {
            transform.localScale = new Vector2(1, 1);
            // _localPosition.x = _xOffset;
        }
        transform.localPosition = _localPosition;
    }

    public void Shoot()
    {
        Instantiate(_projectile, _barrel.position, transform.rotation);
        MusicManager.Instance.fXPlayer.PlayOneShot(_shootingSound, SaveManager.Instance.fXVolume);
    }

    IEnumerator Shooting()
    {
        Shoot();
        yield return _cooldownTimer;
        StartCoroutine("Shooting");
    }
}
