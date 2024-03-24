using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Cached Components

    private Rigidbody2D _rb;
    private Animator _animator;

    [SerializeField] private LayerMask _groundMask;

    #endregion

    #region Fields

    public bool _isGrounded => _rb.IsTouchingLayers(_groundMask);

    [Header("Player Config")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _rollForce = 5.0f;

    private bool _canMove = true;
    private bool _canRoll = true;
    private bool _rolling = false;
    private float _horizontal;
    private float _timeSinceAttack = 0.0f;
    private int _currentAttack = 0;

    [SerializeField] private float _rollDuration = 0.5f;
    [SerializeField] private float _rollCooldown = 1.0f;

    #endregion

    #region MonoBehaviour Methods

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        Inputs();
        FlipSprite();
        UpdateAnimator();

        _animator.SetFloat("yVelocity", _rb.velocity.y);
        _animator.SetBool("isGrounded", _isGrounded);

        _timeSinceAttack += Time.deltaTime;
    }

    #endregion

    #region Private Methods

    private void PlayerMovement()
    {
        if (!_rolling && _canMove)
        {
            Vector2 playerVelocity = new Vector2(_horizontal * _moveSpeed, _rb.velocity.y);

            _rb.velocity = playerVelocity;
        }

    }

    private void Inputs()
    {
        _horizontal = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift) && _canRoll)
        {
            StartCoroutine(Roll());
        }

        if (_animator.GetBool("IdleBlock") && _isGrounded)
        {
            _rb.velocity = Vector2.zero;
        }


        Jump();
        Block();
        Attack();
    }

    private void FlipSprite()
    {
        if (_horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (_horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    private void UpdateAnimator()
    {
        if (_horizontal != 0)
        {
            _animator.SetBool("Run", true);
        }
        else
        {
            _animator.SetBool("Run", false);
        }
    }
    private void Block()
    {
        if (Input.GetMouseButtonDown(1) && !_rolling)
        {
            _canMove = false;
            _animator.SetTrigger("Block");
            _animator.SetBool("IdleBlock", true);


        }
        else if (Input.GetMouseButtonUp(1))
        {
            _animator.SetBool("IdleBlock", false);
            _canMove = true;
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && !_rolling && _isGrounded && !_animator.GetBool("IdleBlock"))
        {
            _animator.SetTrigger("Jump");

            _rb.velocity = Vector2.zero;

            _rb.velocity += new Vector2(0f, _jumpForce);
        }
    }


    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && _timeSinceAttack > 0.25f && !_rolling)
        {
            _currentAttack++;

            if (_currentAttack > 3)
            {
                _currentAttack = 1;
            }

            if (_timeSinceAttack > 1.0f)
            {
                _currentAttack = 1;
            }

            _animator.SetTrigger("Attack" + _currentAttack);

            _timeSinceAttack = 0.0f;
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator Roll()
    {
        _canRoll = false;
        _rolling = true;
        _animator.SetTrigger("Roll");
        _rb.velocity = new Vector2(transform.localScale.x * _rollForce, 0f);
        yield return new WaitForSeconds(_rollDuration);
        _rolling = false;
        yield return new WaitForSeconds(_rollCooldown);
        _canRoll = true;
    }

    #endregion
}
