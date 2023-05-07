using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class Mummy : MonoBehaviour, IDamageable
{

    [SerializeField] private GameObject _cat;
    [SerializeField] private float distanceWalk;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attack;
    [SerializeField] private GameObject Boss;
    [SerializeField] private float distanseAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    private PolygonCollider2D pol;
    private CapsuleCollider2D cap;
    private bool damageNow = false;
    public enum MovementState { stay, walk, attake, hurt, death };
    public MovementState stateMommy;
    private Animator _bossAnimator;
    private Vector3 coordinates;
    private Rigidbody2D _rb;
    private Vector3 delta;
    private Animator animator;
    private bool rotation = true;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        coordinates = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        pol = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        _bossAnimator = Boss.GetComponent<Animator>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        coordinates.x = Boss.transform.position.x;
        coordinates.y = 0;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MummyDeath"))
        {
            Vector2 direction = new Vector2(0, 0);
            if (Vector3.Distance(_cat.transform.position, coordinates) < distanceWalk)
            {
                direction.x = _cat.transform.position.x > transform.position.x ? 1 : -1;
                stateMommy = MovementState.walk;
            }
            else if (Vector3.Distance(_cat.transform.position, coordinates) >= distanceWalk &&
                     Vector3.Distance(coordinates, transform.position) > 0.1)
            {
                direction.x = (coordinates.x - transform.position.x);
                direction.y = 0;
                if (_bossAnimator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
                    stateMommy = MovementState.walk;
            }
            else
                stateMommy = MovementState.stay;
            Attack();
            if (damageNow && HP > 0)
            {
                stateMommy = MovementState.hurt;
                damageNow = false;
            }
            else if (damageNow && HP <= 0)
                stateMommy = MovementState.death;
            Flip(direction.x);
            _rb.velocity = direction * speed;
            animator.SetInteger("state", (int)stateMommy);
        }
        else
        {
            pol.enabled = true;
            cap.enabled = false;
        }
    }

    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MummyAttack"))
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
            if (hitCat.Length > 0)
                stateMommy = MovementState.attake;
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);

        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        damageNow = true;
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
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position, distanseAttack);
    }
}
