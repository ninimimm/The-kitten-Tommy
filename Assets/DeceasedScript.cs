using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeceasedScript : MonoBehaviour, IDamageable
{
    [SerializeField] private float HP = 3;
    [SerializeField] private GameObject target;
    private Animator _animator;
    public enum MovementState { Deceased_Idle, Deceased_Run, Deceased_Attack, Deceased_Dead, Deceased_Hurt };
    public static MovementState _stateDeceased;
    private SpriteRenderer _deceased;
    public float distanseAttack = 0.2f;
    public Transform attack;
    public LayerMask catLayer;
    public int takeDamage = 1;
    private bool damageNow = false;
    private PolygonCollider2D _col;
    private Animator targetAnimation;
    const float speedMove = 1.0f;
    private Rigidbody2D _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _deceased = GetComponent<SpriteRenderer>();
        _col = GetComponent<PolygonCollider2D>();
        targetAnimation = target.GetComponent<Animator>();
    }

    void Update()
    {
        float direction = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) < 3)
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Sign(direction) * speedMove * Time.deltaTime;
            transform.position = pos;
        }
        if (transform.position.x < 35.02)
            transform.position = new Vector3(35.1f, transform.position.y, transform.position.z);
       
        if (_rb.velocity.x > 0)
        {
            _stateDeceased = MovementState.Deceased_Run;
            _deceased.flipX = true;
        }
        else if (_rb.velocity.x < 0)
        {
            _stateDeceased = MovementState.Deceased_Run;
            _deceased.flipX = false;
        }
        else
            _stateDeceased = MovementState.Deceased_Idle;
        if (Vector2.Distance(transform.localPosition, target.transform.localPosition) < 1)
        {
            Attake();
            //_stateDeceased = MovementState.Deceased_Attack;
        }
        if (damageNow)
        {
            _stateDeceased = MovementState.Deceased_Hurt;
            damageNow = false;
            //  HP -= 1;
        }

        _animator.SetInteger("state", (int)_stateDeceased);
    }

    void Attake()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("attake"))
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
            if (hitCat.Length > 0)
            {
                _stateDeceased = MovementState.Deceased_Attack;
                _animator.SetInteger("state", (int)_stateDeceased);
            }
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(takeDamage);
        }
    }
    public void TakeDamage(int damage)
    {
        damageNow = true;
        HP -= damage;
        if (HP <= 0)
        {
            _stateDeceased = MovementState.Deceased_Dead;
            transform.position = new Vector3(-100, 0, 0);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position, distanseAttack);
    }
}