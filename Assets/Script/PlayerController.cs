using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpSpeed;

    [Header("Ground Check")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    [Header("Roll Settings")]
    [SerializeField] private float rollSpeed = 8f;
    [SerializeField] private float rollDuration = 0.4f;

    private Rigidbody2D _rigid;
    private Animator _anim;

    private bool _isGrounded;
    private bool _isJumped;
    private bool _Rolling;

    private Vector2 _moveInput;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 즉시 좌우 반전
        if (_moveInput.x > 0.1f)
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        else if (_moveInput.x < -0.1f)
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);

        _anim.SetFloat("Speed", Mathf.Abs(_moveInput.x));
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetBool("IsJumped", _isJumped);
        _anim.SetBool("Rolling", _Rolling);

        if (_isJumped && _rigid.linearVelocity.y <= 0)
        {
            _isJumped = false;
        }
    }

    private void FixedUpdate()
    {
        // 일반 이동
        float targetSpeed = _moveInput.x * _moveSpeed;
        _rigid.linearVelocity = new Vector2(
            Mathf.Lerp(_rigid.linearVelocity.x, targetSpeed, 0.2f),
            _rigid.linearVelocity.y);
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;
        if (!_isGrounded) return;

        _isJumped = true;
        _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
    }

}
