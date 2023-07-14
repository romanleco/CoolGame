using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFly : Enemy
{
    private WaitForSeconds _colorTimer = new WaitForSeconds(0.15f);
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movementVector;
    private Rigidbody2D _rb;
    private bool _isFollowingPlayer;
    private float _speed;
    private bool _attacking;
    private bool _attackReady = true;
    [SerializeField] private GameObject[] _resources = new GameObject[4];
    [SerializeField] private EnemyGun _enemyGunScr;
    private bool _canBeDamaged;
    private Vector2 _startingPosition;
    [SerializeField] private GameObject _explosionPrefab;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _explosionSound;
    protected override void Start()
    {
        Health = 5;

        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("EnemyFly::Start() SpriteRenderer _spriteRenderer is null");
        }

        _rb = gameObject.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.LogError("EnemyFly::Start() Rigidbody2D is null");
        }

        _speed = Random.Range(2, 5);

        _startingPosition = this.transform.position;
    }

    void Update()
    {
        if(_isFollowingPlayer)
        {
            CalculateMovement();
        }
    }

    protected void CalculateMovement()
    {
        if(_attackReady)
        {
            if(_attacking)
            {
                _movementVector.x = (GameManager.Instance.playerPosition.x - transform.position.x) * (_speed * 1.4f);
                _movementVector.y = (GameManager.Instance.playerPosition.y - transform.position.y) * (_speed * 1.4f);
            }
            else if(!_attacking)
            {
                if(Vector2.Distance(GameManager.Instance.playerPosition, transform.position) < 3)
                {
                    StartCoroutine(Attack());
                }
                else
                {
                    _movementVector.x = (GameManager.Instance.playerPosition.x - transform.position.x) * 0.75f;
                    _movementVector.y = (GameManager.Instance.playerPosition.y - transform.position.y) * 0.75f;
                }
            }
        }
        else
        {
            if(transform.position.y > (_startingPosition.y + 0.1f))
            {
                _movementVector.y = -_speed;
            }
            else if(transform.position.y < (_startingPosition.y - 0.1f))
            {
                _movementVector.y = _speed;
            }

            SideToSide();
        }

        _rb.velocity = _movementVector;
    }

    IEnumerator Attack()
    {
        _attacking = true;
        yield return new WaitForSeconds(1f);
        _attacking = false;
        StartCoroutine(AttackCooldown());
    }

    IEnumerator AttackCooldown()
    {
        _attackReady = false;
        yield return new WaitForSeconds(3f);
        _attackReady = true;
    }

    private void SideToSide()
    {
        if(Mathf.Abs(transform.position.x - GameManager.Instance.playerPosition.x) > 0.8f && _isFollowingPlayer)
        {
            if(transform.position.x - GameManager.Instance.playerPosition.x < 0)
            {
                _movementVector.x = _speed;
                _spriteRenderer.flipX = false;
            }
            else
            {
                _movementVector.x = -_speed;
                _spriteRenderer.flipX = true;
            }
        }
    }

    public override void ReceiveDamage(int damage)
    {
        if(_canBeDamaged)
        {
            StartCoroutine(ChangeColor());
            base.ReceiveDamage(damage);
            MusicManager.Instance.fXPlayer.PlayOneShot(_hitSound, SaveManager.Instance.fXVolume);
        }
    }

    protected override void Die()
    {
        transform.parent.parent.GetComponent<MapVariant>().CheckRoomClear(this);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        MusicManager.Instance.fXPlayer.PlayOneShot(_explosionSound, SaveManager.Instance.fXVolume);
        DropResources();
        base.Die();
    }

    private void DropResources()
    {
        if(Random.Range(0, 3) > 1)
        {
            Instantiate(_resources[Random.Range(0, _resources.Length)], transform.position, Quaternion.identity);
        }
    }

    IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.red;
        yield return _colorTimer;
        _spriteRenderer.color = Color.white;
    }

    IEnumerator StartFollowingPlayer()
    {
        yield return new WaitForSeconds(0.8f);
        if(_enemyGunScr != null)
        {
            _enemyGunScr.StartCoroutine("Shooting");
        }
        _isFollowingPlayer = true;
    }

    public void ActivateEnemy()
    {
        StartCoroutine("StartFollowingPlayer");
        _canBeDamaged = true;
    }
}
