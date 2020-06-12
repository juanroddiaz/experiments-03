using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CollisionEventLogic _footColliderLogic;
    [SerializeField]
    private CollisionEventLogic _frontColliderLogic;
    [SerializeField]
    private Collider2D _headBumperCollider;
    [Header("Physics")]
    [SerializeField]
    private float _moveSpeed = 3.5f;
    [SerializeField]
    private float _jumpSpeed = 7.5f;
    [SerializeField]
    private float _wallSlideSpeed = -1.0f;

    public bool IsGrounded { get; private set; }
    public bool IsFacingWall { get; private set; }
    public bool IsJumping { get; private set; }
    public bool IsFalling { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private bool _mustJump = false;
    private Vector2 _speed = Vector2.zero;

    public void Initialize()
    {
        var footData = new CollisionEventData
        {
            CollisionEnterAction = OnFootCollisionEnter,
            CollisionExitAction = OnFootCollisionExit
        };
        _footColliderLogic.Initialize(footData);
        var frontData = new CollisionEventData
        {
            CollisionEnterAction = OnFrontCollisionEnter,
            CollisionExitAction = OnFrontCollisionExit
        };
        _frontColliderLogic.Initialize(frontData);

        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void OnFootCollisionEnter(Transform t)
    {
        IsGrounded = true;
        IsFalling = false;
    }

    private void OnFootCollisionExit(Transform t)
    {
        IsGrounded = false;
    }

    private void OnFrontCollisionEnter(Transform t)
    {
        IsFacingWall = true;
    }

    private void OnFrontCollisionExit(Transform t)
    {
        IsFacingWall = false;
    }

    public void OnTapDown()
    {
        if (IsGrounded)
        {
            // simple jump
            Debug.Log("Jump");
            _mustJump = true;
            return;
        }
        else
        {
            if (IsFacingWall)
            {
                // rotate and jump
                transform.right *= -1.0f;
                IsFalling = false;
                _mustJump = true;
            }
        }        
    }

    private void FixedUpdate()
    {
        var _speed = _rigidbody2D.velocity;
        if (!IsFacingWall)
        {
            _speed.x = transform.right.x * _moveSpeed;
        }

        if (_mustJump)
        {
            //_rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            _speed.y = _jumpSpeed;
            IsJumping = true;
            _headBumperCollider.enabled = true;
        }

        if (IsJumping && _speed.y < 0.0f)
        {
            IsJumping = false;
            _headBumperCollider.enabled = false;
            IsFalling = true;
        }

        if (IsFalling && IsFacingWall)
        {
            _speed.y = _wallSlideSpeed;
        }

        _mustJump = false;
        _rigidbody2D.velocity = _speed;
    }
}
