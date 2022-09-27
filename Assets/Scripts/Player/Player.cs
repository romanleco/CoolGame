using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public int Health {get; private set;}
    [SerializeField] private bool _dashing;
    [SerializeField] private bool _canJump = true, _canDash = true;
    [SerializeField] private Vector2 _movementVector;
    private bool _facingRight;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;

    [Header("Movement")]
    [SerializeField] private float _speed, _jumpforce, _gravity, _maxFallingSpeed, _dashingSpeed;
    private float _horizontal, _vertical;
    private WaitForSeconds _resetJumpTimer = new WaitForSeconds(0.1f);
    private WaitForSeconds _resetDashTimer = new WaitForSeconds(1.2f);
    private WaitForSeconds _dashTimer = new WaitForSeconds(0.15f);

    [Header("Ground Check")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector2 _rayOrigin, _rayDirection;
    [SerializeField] private float _rayDistance, _rayOffset;
    [SerializeField] private LayerMask _rayLayerMask;
    
    private bool wBOneUpgradeOne;
    private bool wBOneUpgradeTwo;
    private bool wBOneUpgradeThree;

    void Start()
    {
        Health = 100;
        _speed = 26;

        _rb = gameObject.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.LogError("Player::Start() _rb Rigidbody is null");
        }

        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_spriteRenderer == null)
        {
            Debug.LogError("Player::Start() _spriteRenderer SpriteRenderer is null");
        }

        GameManager.Instance.SetPlayerDamagableInterface(GetComponent<IDamagable>());

        _canJump = true;

        _rayDirection = Vector2.down;
        // _rayOffset = -0.64f;
        _rayDistance = 0.2f;

        DataContainer data = SaveManager.Instance.Load();

        if(data.wBOneUpgOneUnlocked == true)
        {
            Health = 150;
        }

        if(data.wBOneUpgTwoUnlocked == true)
        {
            _resetDashTimer = new WaitForSeconds(0.6f);
        }

        if(data.wBOneUpgThreeUnlocked == true)
        {
            _speed = 32;
        }
    }

    void Update()
    {
        CalculateMovement();
        GroundCheck();
        GameManager.Instance.UpdatePlayerPosition(transform.position);
        FacingDirection();
    }

    public virtual void ReceiveDamage(int damage)
    {
        Health -= damage;
        Debug.Log(Health);
        if(Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log("Dead");
        Destroy(this.gameObject);
    }

    private void CalculateMovement()
    {
        _horizontal = Input.GetAxis("Horizontal");
        // _vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.LeftShift) && !_dashing && _canDash)
        {
            _movementVector.y = 0;
            StartCoroutine("DashRoutine");
        }

        if(!_dashing)
        {
            _movementVector.x = _horizontal * _speed;

            if(_isGrounded)
            {
                if(Input.GetKeyDown(KeyCode.Space) && _canJump)
                {
                    _movementVector.y = _jumpforce;
                    StartCoroutine("JumpReset");
                }
            }
            else if(!_isGrounded && _dashing == false)
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
        }

        _rb.velocity = _movementVector;
    }

    private void GroundCheck()
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

    private void FacingDirection()
    {
        if(_horizontal > 0)
        {
            _facingRight = true;
            _spriteRenderer.flipX = false;
        }
        else if(_horizontal < 0)
        {
            _facingRight = false;
            _spriteRenderer.flipX = true;
        }
    }

    private IEnumerator DashRoutine()
    {
        _dashing = true;
        if(_facingRight)
        {
            _movementVector.x = _dashingSpeed;
        }
        else
        {
            _movementVector.x = -_dashingSpeed;
        }
        yield return _dashTimer;
        _dashing = false;
        StartCoroutine("DashReset");
    }

    private IEnumerator DashReset()
    {
        _canDash = false;
        yield return _resetDashTimer;
        _canDash = true;
    }

    private IEnumerator JumpReset()
    {
        _canJump = false;
        yield return _resetJumpTimer;
        _canJump = true;
    }

}
