using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MovePerson : MonoBehaviour
{
    // Start is called before the first frame update
    // Add a jumpForce variable to control the jump strength
    private Rigidbody2D _rb; // Add a RigidBody variable
    private float move;
    private MovementState _state;

    private Animator _animator;
    private enum MovementState { Stay, Run, jumpup, jumpdown };
    private SpriteRenderer Cat;
    [SerializeField] public float speed = 100.0f;
    [SerializeField] public float jumpForce = 1f;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Initialize the RigidBody variable
        Cat = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    // Add a new method to check if the character is grounded
    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(move, 0, 0)*speed*Time.deltaTime;
        if (Input.GetButtonDown("Jump") && CanJump())
            _rb.AddForce(new Vector2(0,jumpForce),ForceMode2D.Impulse);
        SwitchAnimation();
    }

    private bool CanJump() => _state != MovementState.jumpup && _state != MovementState.jumpdown;

    private void SwitchAnimation()
    {
        if (move > 0)
        {
            _state = MovementState.Run;
            Cat.flipX = false;
        }
        else if (move < 0)
        {
            _state = MovementState.Run;
            Cat.flipX = true;
        }
        else
            _state = MovementState.Stay;

        if (_rb.velocity.y > .001f)
            _state = MovementState.jumpup;
        else if (_rb.velocity.y < -.001f)
            _state = MovementState.jumpdown;

        _animator.SetInteger("State", (int)_state);
    }
}