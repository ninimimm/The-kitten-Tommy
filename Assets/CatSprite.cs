using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatSprite : MonoBehaviour
{
    // Start is called before the first frame update
    // Add a jumpForce variable to control the jump strength
    private Rigidbody2D _rb; // Add a RigidBody variable
    private float move;
    private MovementState _state;
    private bool rotation = true;
    private Animator _animator;
    private enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage };
    private SpriteRenderer Cat;
    private Animator _snakeAnimator;
    [SerializeField] private float HP = 10;
    [SerializeField] public float speed = 100.0f;
    [SerializeField] public float jumpForce = 1f;
    [SerializeField] public GameObject Snake;
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>(); // Initialize the RigidBody variable
        Cat = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _snakeAnimator = Snake.GetComponent<Animator>();
        transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
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
            if (rotation)
            {
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.y);
                rotation = false;
            }
        }
        else if (move < 0)
        {
            _state = MovementState.Run;
            if (!rotation)
            {
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.y);
                rotation = true;
            }
        }
        else
            _state = MovementState.Stay;
        if (_rb.velocity.y > .001f)
            _state = MovementState.jumpup;
        else if (_rb.velocity.y < -.001f)
            _state = MovementState.jumpdown;
        if (Vector2.Distance(transform.position,Snake.transform.position) < 1)
        {
            _state = MovementState.damage;
            HP -= 1;
        }
        if (Input.GetButton("Fire1"))
            _state = MovementState.hit;
        if (HP == 0)
        {
            HP = 100;
            transform.position = new Vector3(1, -0.6f, 0);
        }
        _animator.SetInteger("State", (int)_state);
    }
}