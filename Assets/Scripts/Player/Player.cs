using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamagable
{
    public int Health {get; private set;}
    private bool _canBeDamaged = true;
    [SerializeField] private bool _dashing;
    [SerializeField] private bool _canJump = true, _canDash = true;
    [SerializeField] private Vector2 _movementVector;
    private bool _facingRight;
    private SpriteRenderer _spriteRenderer;
    private Rigidbody2D _rb;
    [SerializeField] private GameObject[] _resources = new GameObject[4];

    [Header("Movement")]
    [SerializeField] private float _speed, _jumpforce, _gravity, _maxFallingSpeed, _dashingSpeed;
    private float _horizontal, _vertical;
    private WaitForSeconds _resetJumpTimer = new WaitForSeconds(0.1f);
    private WaitForSeconds _resetDashTimer = new WaitForSeconds(1.5f);
    private WaitForSeconds _dashTimer = new WaitForSeconds(0.15f);
    private bool _alreadyDamaged;
    private WaitForSeconds _damageCooldown = new WaitForSeconds(0.3f);

    [Header("Ground Check")]
    [SerializeField] private bool _isGrounded;
    [SerializeField] private Vector2 _rayOrigin, _rayDirection;
    [SerializeField] private float _rayDistance, _rayOffset;
    [SerializeField] private LayerMask _rayLayerMask;
    
    private bool wBOneUpgradeOne;
    private bool wBOneUpgradeTwo;
    private bool wBOneUpgradeThree;

    [Header("Visual Effects")]
    [SerializeField] private TrailRenderer _trailRenderer;
    private Animator _animator;
    private WaitForSeconds _colorOnDamageTimer = new WaitForSeconds(0.2f);
    [SerializeField] private PlayerUI _playerUI;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip _hitHalf;
    [SerializeField] private AudioClip _hitFull;
    [SerializeField] private AudioClip _propulsion;
    [SerializeField] private AudioClip _jump;
    [SerializeField] private AudioSource _playerAudioSource;

    void Start()
    {
        Health = 5;
        _speed = 22;

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

        _animator = gameObject.GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Player::Start() _animator Animator is null");
        }

        if(_playerUI == null)
        {
            Debug.LogError("Player::Start() _playerUI Canvas is null");
        }

        GameManager.Instance.SetPlayerDamagableInterface(GetComponent<IDamagable>());

        _canJump = true;

        _rayDirection = Vector2.down;
        // _rayOffset = -0.64f;
        _rayDistance = 0.2f;

        DataContainer data = SaveManager.Instance.Load();

        if(data.wBOneUpgOneUnlocked == true)
        {
            Health = 7;
            _playerUI.HealthExtended(true);
        }
        _playerUI.UpdateHealth(Health);

        if(data.wBOneUpgTwoUnlocked == true)
        {
            _resetDashTimer = new WaitForSeconds(1f);
        }

        if(data.wBOneUpgThreeUnlocked == true)
        {
            _speed = 26;
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
        if(_canBeDamaged && _alreadyDamaged == false)
        {
            Health -= damage;
            Debug.Log(Health);
            if(Health <= 0)
            {
                Health = 0;
                Die();
            }
            else
            {
                StartCoroutine("ColorOnDamage");
                // AudioSource.PlayClipAtPoint(_hitHalf, Vector2.zero, SaveManager.Instance.fXVolume);
                _playerAudioSource.PlayOneShot(_hitHalf, SaveManager.Instance.fXVolume);
                StartCoroutine("Damagable");
            }
            _playerUI.UpdateHealth(Health);
        }
    }

    IEnumerator ColorOnDamage()
    {
        _spriteRenderer.color = Color.red;
        yield return _colorOnDamageTimer;
        _spriteRenderer.color = Color.white;
    }

    IEnumerator Damagable()
    {
        _alreadyDamaged = true;
        yield return _damageCooldown;
        _alreadyDamaged = false;
    }

    protected virtual void Die()
    {
        MusicManager.Instance.fXPlayer.PlayOneShot(_hitFull, SaveManager.Instance.fXVolume);
        DropResources();
        SceneManager.Instance.ChangeSceneWithDelay(1f, "BaseScene");
        Destroy(this.gameObject);
    }

    private void DropResources()
    {
        for(int i = 0; i < GameManager.Instance.metalPlates; i++)
        {
            Instantiate(_resources[0], transform.position, Quaternion.identity);
        }

        for(int i = 0; i < GameManager.Instance.energyCores; i++)
        {
            Instantiate(_resources[1], transform.position, Quaternion.identity);
        }

        for(int i = 0; i < GameManager.Instance.gears; i++)
        {
            Instantiate(_resources[2], transform.position, Quaternion.identity);
        }

        for(int i = 0; i < GameManager.Instance.circuitBoards; i++)
        {
            Instantiate(_resources[3], transform.position, Quaternion.identity);
        }
    }

    private void CalculateMovement()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        if(Mathf.Abs(_horizontal) > 0.1f)
        {
            _animator.SetBool("Walking", true);
        }
        else
        {
            _animator.SetBool("Walking", false);
        }
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
                    _playerAudioSource.PlayOneShot(_jump, SaveManager.Instance.fXVolume);
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
                _animator.SetBool("Jumping", false);
            }
        }
        else
        {
            _isGrounded = false;
            _animator.SetBool("Jumping", true);
        }
        // Debug.DrawRay(_rayOrigin, _rayDirection * _rayDistance, Color.magenta);
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
        _playerAudioSource.PlayOneShot(_propulsion, SaveManager.Instance.fXVolume);
        _trailRenderer.emitting = true;
        _canBeDamaged = false;
        if(_facingRight)
        {
            _movementVector.x = _dashingSpeed;
        }
        else
        {
            _movementVector.x = -_dashingSpeed;
        }
        yield return _dashTimer;
        _trailRenderer.emitting = false;
        _canBeDamaged = true;
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
