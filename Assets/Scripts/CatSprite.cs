using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CatSprite : MonoBehaviour
{
    private Rigidbody2D _rb;
    private float move;
    public static MovementState _stateCat;
    private bool rotation = true;
    public static Animator _animator;
    public static Animation _Animation;
    public enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage };
    private SpriteRenderer Cat;
    private Animator _snakeAnimator;
    private bool IsSnakeAttack;
    private bool IsLastSnakeAttack;
    private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public GameObject Snake;
    [SerializeField] private HealthBar _healthBar;
    public Transform smallAttack;
    public float distanseSmallAttack = 0.2f;
    public LayerMask enemyLayers;
    public int takeDamage = 1;
    private float speedMultiplier = 1f;
    private bool damageNow = false;

    public int money = 0;

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        Cat = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _snakeAnimator = Snake.GetComponent<Animator>();
        _Animation = GetComponent<Animation>();
        transform.Rotate(0f,180f,0f);
        _healthBar.SetMaxHealth(maxHP);
        HP = maxHP;
    }

    private void Update()
    {
        move = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(move, 0, 0) * speed * speedMultiplier * Time.deltaTime;
        if (Input.GetButtonDown("Jump") && CanJump())
            _rb.AddForce(new Vector2(0, jumpForce - speedMultiplier*2), ForceMode2D.Impulse);
        SwitchAnimation();
    }

    private bool CanJump() => _stateCat != MovementState.jumpup && _stateCat != MovementState.jumpdown;

    private void SwitchAnimation()
    {
        if (move > 0)
        {
            _stateCat = MovementState.Run;
            if (rotation)
            {
                transform.Rotate(0f,180f,0f);
                rotation = false;
            }
        }
        else if (move < 0)
        {
            _stateCat = MovementState.Run;
            if (!rotation)
            {
                transform.Rotate(0f,180f,0f);
                rotation = true;
            }
        }
        else
            _stateCat = MovementState.Stay;

        if (_rb.velocity.y > .001f) _stateCat = MovementState.jumpup;
        else if (_rb.velocity.y < -.001f) _stateCat = MovementState.jumpdown;
        
        if (Input.GetKeyDown(KeyCode.Q))
            Attake();

        if (damageNow)
        {
            _stateCat = MovementState.damage;
            damageNow = false;
        }
        _animator.SetInteger("State", (int)_stateCat);
    }

    void Attake()
    {
        if (_stateCat == MovementState.damage) return;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            _stateCat = MovementState.hit;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(smallAttack.position, distanseSmallAttack, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(takeDamage);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (smallAttack.position == null)
            return;
        Gizmos.DrawWireSphere(smallAttack.position,distanseSmallAttack);
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
        if (HP <= 0)
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            transform.position = new Vector3(1, 0, 0);
        }
            
    }
}