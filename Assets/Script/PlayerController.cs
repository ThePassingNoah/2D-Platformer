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
    private bool _isTurning = false;

    private float _lastMoveX = 1f;
    private float _queuedTurnX = 1f;

    private Vector2 _moveInput;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _lastMoveX = transform.localScale.x > 0 ? 1 : -1;
    }

    private void Update()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        _anim.SetFloat("Speed", Mathf.Abs(_moveInput.x));
        _anim.SetBool("IsGrounded", _isGrounded);
        _anim.SetBool("IsJumped", _isJumped);
        _anim.SetBool("Rolling", _Rolling);

        if (_isJumped && _rigid.linearVelocity.y <= 0)
        {
            _isJumped = false;
        }

        if (_isJumped || _Rolling)
        {
            _isTurning = false;
        }

        HandleTurning();
    }
    private void FixedUpdate()
    {
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

    public void OnRoll(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        if (_Rolling) return;
        if (!_isGrounded) return;

        StartCoroutine(RollRoutine());
    }

    private IEnumerator RollRoutine()
    {
        _Rolling = true;

        // 무적 효과 적용 (필요 시 Collider Off 또는 Layer 변경)
        // 예) gameObject.layer = LayerMask.NameToLayer("Dodge");

        float timer = 0f;

        // 플레이어 바라보는 방향
        float facing = transform.localScale.x > 0 ? 1 : -1;

        bool isMoving = Mathf.Abs(_moveInput.x) > 0.1f;

        while (timer < rollDuration)
        {
            timer += Time.deltaTime;

            if (!isMoving)
            {
                _rigid.linearVelocity = new Vector2(facing * rollSpeed, _rigid.linearVelocity.y);
            }

            yield return null;
        }

        // 무적 제거
        // 예) gameObject.layer = LayerMask.NameToLayer("Player");

        _Rolling = false;
    }

    private void HandleTurning()
    {
        float moveX = _moveInput.x;

        if (Mathf.Abs(moveX) < 0.1f) return;

        float inputDirection = moveX > 0 ? 1 : -1;

        float currentFacing = Mathf.Sign(transform.localScale.x);

        if (!_isTurning && inputDirection != currentFacing)
        {
            _queuedTurnX = inputDirection; 
            _lastMoveX = inputDirection;   

            _isTurning = true;
            _anim.SetTrigger("Turn");
        }
    }
    public void OnTurnFinished()
    {
        _anim.ResetTrigger("Turn");

        float dir = _queuedTurnX;
        transform.localScale = new Vector3(dir * 2.5f, 2.5f, 1);

        _isTurning = false;
    }
}
