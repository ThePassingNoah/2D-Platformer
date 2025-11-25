using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;

    private Rigidbody2D _rigid;
    private bool _isGrounded;
    private bool _isJumped;
    private bool _Rolling;
    private Vector2 _moveInput;
    private Animator _anim;

    private void Start()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
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
    }
    //입력 값을 바탕으로 이동
    private void FixedUpdate()
    {
        _rigid.linearVelocity = new Vector2(
            _moveInput.x * _moveSpeed,
            _rigid.linearVelocity.y);
    }
    // 사용자 입력을 받아옴
    public void OnMove(InputAction.CallbackContext ctx)
    {
        _moveInput = ctx.ReadValue<Vector2>();
    }
    //점프 입력이 들어오면 위쪽 방향으로 힘을 줘서 점프 구현
    public void OnJump(InputAction.CallbackContext ctx)
    {
        //땅을 밟고 있을 때만 점프
        if (_isGrounded)
        {
            _isJumped = true;
            _rigid.AddForce(Vector2.up * _jumpSpeed, ForceMode2D.Impulse);
        }
    }
}
