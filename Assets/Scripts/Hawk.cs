using System;
using UnityEngine;

public class Hawk : MonoBehaviour, IDamageable
{
    private Rigidbody2D _rigidbody2D;
    public enum MovementState { stay, walk, fly, attack, hurt, death }
    public static MovementState _stateHawk;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float attackRange = 1.0f;
    [SerializeField] private int damage = 10;
    [SerializeField] private float followRange = 5.0f;
    [SerializeField] private float minY = -2.0f;
    [SerializeField] private float maxY = 2.0f;
    [SerializeField] private GameObject Cat;
    [SerializeField] private float maxHP;
    private float HP;
    public float distanseAttack = 0.2f;
    public Transform attack;
    public LayerMask catLayer;
    private CatSprite _catSprite;
    private Animator _animator;
    private bool damageNow = false;

    void Start()
    {
        HP = maxHP;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _catSprite = Cat.GetComponent<CatSprite>();
    }

    void Update()
    {
        Move();
        Attack();
    }

    private void Move()
    {
        Vector2 direction = new Vector2(0, 0);

        if (Mathf.Abs(transform.position.y - Cat.transform.position.y) < maxY &&
            Mathf.Abs(transform.position.x - Cat.transform.position.x) < followRange)
        {
            direction.x = Cat.transform.position.x > transform.position.x ? 1 : -1;
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
        if (damageNow)
        {
            _stateHawk = MovementState.hurt;
            damageNow = false;
        }
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
                _stateHawk = MovementState.attack;
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);;
        }
    }
    

    private void Flip(float horizontalDirection)
    {
        if (horizontalDirection > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalDirection < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    public void TakeDamage(float damage)
    {
        HP -= damage;
        damageNow = true;
        if (HP <= 0)
        {
            HP = maxHP;
            transform.position = new Vector3(1, 0, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}