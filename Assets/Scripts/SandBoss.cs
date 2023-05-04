using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

public class SandBoss : MonoBehaviour, IDamageable
{
    
    [SerializeField] private GameObject _cat;
    [SerializeField] private float distanceWalk;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    private PolygonCollider2D pol;
    private CapsuleCollider2D cap;
    private bool damageNow = false;
    public enum MovementState { stay, walk, attake, death, hurt};
    public MovementState stateBoss;
    private Vector3 coordinates;
    private Rigidbody2D _rb;
    private Vector3 delta;
    private Animator animator;
    private bool rotation = true;
    
    public GameObject ballPrefab; // префаб шара-спрайта
    public float attackInterval = 2.0f; // интервал атаки (в секундах)
    private float attackTimer; // таймер для атаки

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = attackInterval;
        HP = maxHP;
        coordinates = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        pol = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            attackTimer -= Time.deltaTime;
            Vector2 direction = new Vector2(0, 0);
            if (Vector3.Distance(_cat.transform.position, transform.position) > distanceWalk)
            {
                direction.x = _cat.transform.position.x > transform.position.x ? 1 : -1;
                stateBoss = MovementState.walk;    
            }
            else
                stateBoss = MovementState.stay;
            if (attackTimer <= 0 && stateBoss != MovementState.attake && Vector3.Distance(_cat.transform.position, transform.position) < distanseAttack)
            {
                stateBoss = MovementState.attake;
                attackTimer = attackInterval;
            }
            if (stateBoss == MovementState.attake)
                Attack();
            if (damageNow && HP > 0)
            {
                stateBoss = MovementState.hurt;
                damageNow = false;
            }
            else if (damageNow && HP <= 0)
                stateBoss = MovementState.death;
            Flip(direction.x);
            _rb.velocity = direction * speed;
            animator.SetInteger("state", (int)stateBoss);
        }
        else
        {
            pol.enabled = false;
            cap.enabled = true;
        }
    }

    void Attack()
    {
        stateBoss = MovementState.attake;
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 5.0f;
        spawnPosition.x += Random.Range(-distanseAttack, distanseAttack);
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
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
}