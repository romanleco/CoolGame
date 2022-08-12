using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private bool _isGrounded;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private Vector2 _movementVector;
    private Rigidbody2D _rb;
    [SerializeField] private float _speed, _jumpforce, _gravity, _maxFallingSpeed;
    private float _horizontal, _vertical;
    private WaitForSeconds _resetTimer = new WaitForSeconds(0.1f);

    [Header("Ground Check")]
    [SerializeField] private Vector2 _rayOrigin, _rayDirection;
    [SerializeField] private float _rayDistance, _rayOffset;
    [SerializeField] private LayerMask _rayLayerMask;

    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        if(_rb == null)
        {
            Debug.LogError("Player::Start() _rb Rigidbody is null");
        }

        _canJump = true;

        _rayDirection = Vector2.down;
        // _rayOffset = -0.64f;
        _rayDistance = 0.2f;
    }

    void Update()
    {
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");

        GroundCheck();

        _movementVector.x = _horizontal * _speed;

        if(_isGrounded)
        {
            if(Input.GetKeyDown(KeyCode.Space) && _canJump)
            {
                _movementVector.y = _jumpforce;
                StartCoroutine("JumpReset");
            }
        }
        else
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

    private IEnumerator JumpReset()
    {
        _canJump = false;
        yield return _resetTimer;
        _canJump = true;
    }

}
