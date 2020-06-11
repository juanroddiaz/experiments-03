using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [SerializeField]
    private CollisionEventLogic _footColliderLogic;
    [SerializeField]
    private CollisionEventLogic _frontColliderLogic;
    [Header("Physics")]
    [SerializeField]
    private float _speed = 2.0f;
    [SerializeField]
    private float _jumpForce = 2.0f;

    public bool IsGrounded { get; private set; }
    public bool IsFacingWall { get; private set; }
    public bool IsJumping { get; private set; }

    private Rigidbody2D _rigidbody2D;
    private bool _mustJump = false;
    private Vector2 _forceToApply = Vector2.zero;

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
        IsJumping = false;
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
        if(IsJumping)
        {
            return;
        }

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
                _mustJump = true;
            }
        }        
    }

    private void FixedUpdate()
    {
        if ((IsGrounded || IsJumping) && !IsFacingWall)
        {
            var speed = _rigidbody2D.velocity;
            speed.x = transform.right.x * _speed;
            _rigidbody2D.velocity = speed;
        }

        if (_mustJump)
        {
            _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
            IsJumping = true;
        }

        _mustJump = false;
        _forceToApply = Vector2.zero;
    }
}
