using System;
using UnityEngine;
using UnityEngine.UI;
public class Hawk : MonoBehaviour, IDamageable
{
    public enum MovementState { stay, walk, fly, attack, hurt, death }
    public static MovementState _stateHawk;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float followRange = 5.0f;
    [SerializeField] private float minY = -2.0f;
    [SerializeField] private float maxY = 2.0f;
    [SerializeField] private GameObject Cat;
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    public float distanseAttack = 0.2f;
    public Transform attack;
    public LayerMask catLayer;
    private bool damageNow = false;
    private CapsuleCollider2D[] caps;

    private Rigidbody2D _rigidbody2D;
    private CatSprite _catSprite;
    private Animator _animator;
    private Vector3 catPosition;

    void Start()
    {
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _catSprite = Cat.GetComponent<CatSprite>();
        caps = GetComponents<CapsuleCollider2D>();
    }

    void Update()
    {
        catPosition = Cat.transform.position; // Store the cat position

        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("hawk_death"))
            Move();
        else
        {
            caps[0].enabled = false;
            caps[1].enabled = true;
            _fill.enabled = false;
            _bar.enabled = false;
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(0, 0);

        if (Mathf.Abs(transform.position.y - Cat.transform.position.y) < maxY &&
            Mathf.Abs(transform.position.x - Cat.transform.position.x) < followRange)
        {
            if (Cat.transform.position.x > transform.position.x + 0.1)
                direction.x = 1;
            else if (Cat.transform.position.x < transform.position.x - 0.1)
                direction.x = -1;
            direction.y = Cat.transform.position.y > transform.position.y ? 0.1f : -0.1f;
            _stateHawk = MovementState.fly;
        }
        else if (Math.Abs(direction.y) < 0.2 )
        {
            _stateHawk = MovementState.stay;
        }

        if (transform.position.y > maxY)
        {
            direction.y = -1;
        }
        else if (transform.position.y < minY)
        {
            direction.y = 1;
        }
        Attack();
        if (damageNow && HP > 0)
        {
            _stateHawk = MovementState.hurt;
            damageNow = false;
        }
        else if (damageNow && HP <= 0)
            _stateHawk = MovementState.death;
        _animator.SetInteger("state", (int)_stateHawk);
        _rigidbody2D.velocity = direction * speed;
        Flip(direction.x);
        
    }

    private void Attack()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("hawk_attack"))
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
            if (hitCat.Length > 0)
            {
                _stateHawk = MovementState.attack;
                _animator.SetInteger("state", (int)_stateHawk);
            }
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);;
        }
    }
    

    private void Flip(float horizontalDirection)
    {
        if (horizontalDirection > 0.9)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalDirection < -0.9)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
    }
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}