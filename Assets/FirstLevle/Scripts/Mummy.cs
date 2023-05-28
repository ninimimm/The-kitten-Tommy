using System;
using UnityEngine;
using UnityEngine.UI;

public class Mummy : MonoBehaviour, IDamageable
{

    [SerializeField] public GameObject _cat;
    [SerializeField] private float distanceWalk;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attack;
    [SerializeField] public GameObject Boss;
    [SerializeField] private float distanseAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxHP;
    [SerializeField] public float HP;
    public GameObject boss;
    public HealthBar _healthBar;
    public Image __fill;
    public Image __bar;
    private Rigidbody2D rbBoss;
    private Animator _bossAnimator;
    public PolygonCollider2D pol;
    public CapsuleCollider2D cap;
    private bool damageNow = false;
    public enum MovementState { stay, walk, attake, hurt, death };
    public MovementState stateMommy;
    private Vector3 coordinates;
    private Rigidbody2D _rb;
    private Vector3 delta;
    public Animator animator;
    public AudioSource _audioSourceMummyHurt;
    public AudioSource _audioSourceMummyAttack;
    private bool isStart = true;
    private MummyData data;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        coordinates = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        pol = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        rbBoss = GetComponent<Rigidbody2D>();
        _bossAnimator = Boss.GetComponent<Animator>();
        cap.enabled = true;
        pol.enabled = false;
        if (!SnakeData.start.Contains(gameObject.name))
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            Save();
            SnakeData.start.Add(gameObject.name);
        }
        Load();
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP); 
    }
    
    public void Save()
    {
        SavingSystem<Mummy,MummyData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Mummy, MummyData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        pol.enabled = data.polyEnabled;
        cap.enabled = data.capEnabled;
        animator.SetInteger("state",data.animatorState);
    }

    // Update is called once per frame
    void Update()
    {
        if (boss.GetComponent<SandBoss>().alive)
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
                    if (Math.Abs(Boss.transform.position.x - transform.position.x) < 1)
                    {
                        stateMommy = MovementState.stay;
                        direction = new Vector2(0, 0);
                    }
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
                __fill.enabled = false;
                __bar.enabled = false;
            }
        }
        else
        {
            if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MummyDeath"))
            {
                Vector2 direction = new Vector2(0, 0);
                if (Vector3.Distance(_cat.transform.position, coordinates) < distanceWalk)
                {
                    direction.x = _cat.transform.position.x > transform.position.x ? 1 : -1;
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
                __fill.enabled = false;
                __bar.enabled = false;
            }
        }
        if (isStart)
        {
            Load();
            isStart = false;
        }
    }
    void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MummyAttack"))
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
            if (hitCat.Length > 0)
            {
                stateMommy = MovementState.attake;
                if (!_audioSourceMummyAttack.isPlaying)
                    _audioSourceMummyAttack.Play();
            }

            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);
        }
    }

    public void TakeDamage(float damage)
    {
        if (!_audioSourceMummyHurt.isPlaying)
            _audioSourceMummyHurt.Play();
        HP -= damage;
        _healthBar.SetHealth(HP);
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
