using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : Enemy
{
    private WaitForSeconds _colorTimer = new WaitForSeconds(0.15f);
    private SpriteRenderer _spriteRenderer;
    private Vector2 _movementVector;
    private Rigidbody2D _rb;
    private bool _isFollowingPlayer;
    [SerializeField] private float _maxFallingSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _speed;
    [SerializeField] private GameObject[] _resources;
    [SerializeField] private Animator _animator;
    private bool _canBeDamaged;
    [SerializeField] private GameObject _explosionPrefab;

    [Header("GroundCheck")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector2 _rayOrigin, _rayDirection;
    [SerializeField] private float _rayDistance, _rayOffset;
    [SerializeField] private LayerMask _rayLayerMask;
    [SerializeField] private EnemyGun _enemyGunScr;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip _explosionSound;
    protected override void Start()
    {
        base.Start();
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("EnemyWalker::Start() SpriteRenderer _spriteRenderer is null");
        }

        _rb = gameObject.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.LogError("EnemyWalker::Start() Rigidbody2D is null");
        }

        _rayDirection = Vector2.down;
        // _rayOffset = -0.64f;
        _rayDistance = 0.05f;

        _speed = Random.Range(5, 9);
    }

    void Update()
    {
        GroundCheck();
        CalculateMovement();
    }

    protected void CalculateMovement()
    {
        if(!_isGrounded)
        {
            if(_movementVector.y > _maxFallingSpeed)
            {
                _movementVector.y -= _gravity * Time.deltaTime;
            }
            else if(_movementVector.y != _maxFallingSpeed)
            {
                _movementVector.y = _maxFallingSpeed;
            }

            if(_animator.GetBool("rolling") == true)
            {
                _animator.SetBool("rolling", false);
            }
        }
        else
        {
            _movementVector.y = 0;

            if(_animator.GetBool("rolling") == false)
            {
                _animator.SetBool("rolling", true);
            }
        }

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

        _rb.velocity = _movementVector;
    }

    protected void GroundCheck()
    {
        _rayOrigin = transform.position;
        _rayOrigin.y += _rayOffset;

        RaycastHit2D hit = Physics2D.Raycast(_rayOrigin, _rayDirection, _rayDistance, _rayLayerMask);
        if(hit.collider != null)
        {
            if(hit.collider.tag == "Floor")
            {
                _isGrounded = true;
            }
        }
        else
        {
            _isGrounded = false;
        }
        Debug.DrawRay(_rayOrigin, _rayDirection * _rayDistance, Color.magenta);
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
