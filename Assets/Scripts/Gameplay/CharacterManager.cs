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
    private float _speed = 2.0f;

    public bool IsGrounded { get; private set; }
    public bool IsFacingWall { get; private set; }

    private Rigidbody2D _rigidbody2D;

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

    private void FixedUpdate()
    {
        if (!IsGrounded && IsFacingWall)
        {
            // if(mustJump)            
            // else if speed Y axis = neg wall slide
        }

        if (IsGrounded && !IsFacingWall)
        {
            _rigidbody2D.MovePosition(transform.position + transform.right * _speed * Time.fixedDeltaTime);
        }
    }
}
