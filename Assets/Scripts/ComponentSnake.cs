using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ComponentSnake : MonoBehaviour, IDamageable
{
    private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] private GameObject target;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    public static Animator _animator;
    public enum MovementState { stay, Walk, attake, death, hurt };
    public static MovementState _stateSnake;
    private SpriteRenderer _snake;
    private PolygonCollider2D _col;
    private Animator targetAnimation;
    private Rigidbody2D _rb;
    public Transform attack;
    public float distanseAttack = 0.2f;
    public LayerMask catLayer;
    public int takeDamage = 1;
    private bool rotation = true;
    private bool damageNow = false;
    private PolygonCollider2D poly;
    private CapsuleCollider2D cap;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _snake = GetComponent<SpriteRenderer>();
        _col = GetComponent<PolygonCollider2D>();
        targetAnimation = target.GetComponent<Animator>();
        poly = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        _healthBar.SetMaxHealth(maxHP);
        HP = maxHP;
    }

    void Update()
    {
        if (transform.position.x > 12.7)
            transform.position = new Vector3(12.67f, transform.position.y, transform.position.z);
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            if (_rb.velocity.x > 0.5)
            {
                if (rotation)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
                    rotation = false;
                }
                _stateSnake = MovementState.Walk;
            }
            else if (_rb.velocity.x < -0.5)   
            {
                if (!rotation)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.y);
                    rotation = true;
                }
                _stateSnake = MovementState.Walk;
            }
            else _stateSnake = MovementState.stay;
            Attake();
            if (damageNow && HP > 0)
            {
                _stateSnake = MovementState.hurt;
                damageNow = false;
            }
            else if (damageNow && HP <= 0)
                _stateSnake = MovementState.death;
            _animator.SetInteger("state", (int)_stateSnake);
        }
        else
        {
            _rb.velocity = new Vector2(0, 0);
            poly.enabled = false;
            cap.enabled = true;
            _fill.enabled = false;
            _bar.enabled = false;
        }
    }

    void Attake()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("attake"))
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
            if (hitCat.Length > 0)
            {
                _stateSnake = MovementState.attake;
                _animator.SetInteger("state", (int)_stateSnake);
            }
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(takeDamage);
        }
    }
    public void TakeDamage(float damage)
    {
        damageNow = true;
        HP -= damage;
        _healthBar.SetHealth(HP);
    }
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}