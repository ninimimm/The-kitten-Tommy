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
    [SerializeField] public float HP;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    public float distanseAttack = 0.2f;
    public Transform attack;
    public LayerMask catLayer;
    private bool damageNow = false;
    public CapsuleCollider2D[] caps;

    private Rigidbody2D _rigidbody2D;
    public Animator _animator;
    private Vector3 catPosition;
    private AudioSource _audioSource;
    private HawkData data;
    private bool isStart = true;
    private Vector2 direction;
    private CatSprite _catSprite;
    private AnimatorStateInfo _stateInfo;

    void Start()
    {
        _catSprite = Cat.GetComponent<CatSprite>();
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        caps = GetComponents<CapsuleCollider2D>();
        if (!HawkData.start.Contains(gameObject.name))
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            Save();
            HawkData.start.Add(gameObject.name);
        }
        Load();
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
        _audioSource = GetComponent<AudioSource>();
    }
    
    public void Save()
    {
        SavingSystem<Hawk,HawkData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Hawk, HawkData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        caps[0].enabled = data.capsFirstEnabled;
        caps[1].enabled = data.capsSecondEnabled;
        _animator.SetInteger("state",data.animatorState);
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        catPosition = Cat.transform.position; // Store the cat position

        if (!_stateInfo.IsName("hawk_death"))
            Move();
        else
        {
            caps[0].enabled = false;
            caps[1].enabled = true;
            _fill.enabled = false;
            _bar.enabled = false;
        }
        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    private void Move()
    {
        direction = new Vector2(0, 0);

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
            _stateHawk = MovementState.stay;

        if (transform.position.y > maxY)
            direction.y = -0.5f;
        else if (transform.position.y < minY)
            direction.y = 0.5f;
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
        if (!_stateInfo.IsName("hawk_attack"))
        {
            if (Physics2D.OverlapCircle(attack.position, distanseAttack, catLayer))
            {
                _stateHawk = MovementState.attack;
                _animator.SetInteger("state", (int)_stateHawk);
                _catSprite.TakeDamage(damage);;
            }
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
        if (!_audioSource.isPlaying)
            _audioSource.PlayOneShot(damageClip);
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}