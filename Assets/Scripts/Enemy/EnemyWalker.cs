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

    [Header("GroundCheck")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector2 _rayOrigin, _rayDirection;
    [SerializeField] private float _rayDistance, _rayOffset;
    [SerializeField] private LayerMask _rayLayerMask;
    [SerializeField] private EnemyGun _enemyGunScr;
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
        }
        else
        {
            _movementVector.y = 0;
        }

        if(Mathf.Abs(transform.position.x - GameManager.Instance.playerPosition.x) > 0.8f && _isFollowingPlayer)
        {
            if(transform.position.x - GameManager.Instance.playerPosition.x < 0)
            {
                _movementVector.x = _speed;
            }
            else
            {
                _movementVector.x = -_speed;
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
        StartCoroutine(ChangeColor());
        base.ReceiveDamage(damage);
    }

    protected override void Die()
    {
        transform.parent.parent.GetComponent<MapVariant>().CheckRoomClear(this);
        base.Die();
    }

    IEnumerator ChangeColor()
    {
        _spriteRenderer.color = Color.white;
        yield return _colorTimer;
        _spriteRenderer.color = Color.red;
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
    }
}
