using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SandBoss : MonoBehaviour, IDamageable
{
    
    [SerializeField] public GameObject _cat;
    [SerializeField] private float distanceWalk;
    [SerializeField] private float distanseStay;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxHP;
    [SerializeField] private float HP;
    public AudioSource _audioBall;
    public float _valueMummy;
    public Canvas _canvasMummy;
    public Image fill;
    public Image bar; 
    public HealthBar _healthBar;
    private PolygonCollider2D pol;
    private CapsuleCollider2D cap;
    private bool damageNow = false;
    public enum MovementState { stay, walk, attake, death, hurt};
    public MovementState stateBoss;
    private Vector3 coordinates;
    private Rigidbody2D _rb;
    private Vector3 delta;
    private Animator animator;
    public bool alive = true;
    
    public GameObject ballPrefab;
    public GameObject MummyPrefab;// префаб шара-спрайта
    public float attackInterval = 2.0f;
    public float spawnInterval = 2.0f;// интервал атаки (в секундах)
    private float attackTimer;
    private float spawnTimer;// таймер для атаки

    // Start is called before the first frame update
    void Start()
    {
        attackTimer = attackInterval;
        spawnTimer = spawnInterval;
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        coordinates = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        pol = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        pol.enabled = true;
        cap.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
        {
            attackTimer -= Time.deltaTime;
            spawnTimer -= Time.deltaTime;
            Vector2 direction = new Vector2(0, 0);
            if (Vector3.Distance(_cat.transform.position, transform.position) > distanceWalk && 
                Vector3.Distance(_cat.transform.position, transform.position) < distanseStay)
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

            if (spawnTimer <= 0)
            {
                Spawn();
                spawnTimer = spawnInterval;
            }
                
            Flip(direction.x);
            _rb.velocity = direction * speed;
            animator.SetInteger("state", (int)stateBoss);
        }
        else
        {
            alive = false;
            pol.enabled = false;
            cap.enabled = true;
            fill.enabled = false;
            bar.enabled = false;
        }
    }

    void Attack()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.y += 5.0f;
        spawnPosition.x = (_cat.transform.position.x<transform.position.x)?Random.Range(-distanseAttack+transform.position.x, transform.position.x-1):Random.Range(transform.position.x+1, transform.position.x+distanseAttack);
        if (ballPrefab == null)
            return;
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        ball.GetComponent<Ball>()._audioSource = _audioBall;
    }
    void Spawn()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += (transform.position.x > _cat.transform.position.x) ? -1 : 1;
        if (MummyPrefab == null)
            return;
        GameObject Mummy = Instantiate(MummyPrefab, new Vector3(spawnPosition.x,spawnPosition.y-1,
            spawnPosition.y), Quaternion.identity);
        Mummy.GetComponent<Mummy>()._cat = _cat;
        Mummy.GetComponent<Mummy>().Boss = gameObject;
        var newCanvas = Instantiate(_canvasMummy,
            new Vector3(spawnPosition.x, spawnPosition.y + _valueMummy, spawnPosition.z), Quaternion.identity);
        var healthBar = newCanvas.GetComponentInChildren<HealthBar>();
        var _fill = healthBar.GetComponentsInChildren<Image>()[0].GetComponent<Image>();
        var _bar = healthBar.GetComponentsInChildren<Image>()[1].GetComponent<Image>();
        healthBar.GetComponent<EnemyHealthBar>()._target = Mummy.transform;
        healthBar.GetComponent<EnemyHealthBar>()._value = _valueMummy;
        Mummy.GetComponent<Mummy>()._healthBar = healthBar;
        Mummy.GetComponent<Mummy>().__fill = _fill;
        Mummy.GetComponent<Mummy>().__bar = _bar;
        Mummy.GetComponent<Mummy>().boss = gameObject;
    }
    
    public void TakeDamage(float damage)
    {
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
}