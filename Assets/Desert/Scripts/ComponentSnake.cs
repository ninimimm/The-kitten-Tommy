using UnityEngine;
using UnityEngine.UI;

public class ComponentSnake : MonoBehaviour, IDamageable
{
    public float HP;
    [SerializeField] private float maxHP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private CatSprite _catSprite;
    [SerializeField] private WriteText _writeText;
    private AudioSource _audioSource;
    public Animator _animator;
    public enum MovementState { stay, Walk, attake, death, hurt };
    public static MovementState _stateSnake;
    private Rigidbody2D _rb;
    public Transform attack;
    public float distanseAttack = 0.2f;
    public LayerMask catLayer;
    public int takeDamage = 1;
    private bool rotation = true;
    private bool damageNow;
    public PolygonCollider2D poly;
    public CapsuleCollider2D cap;
    private SnakeData data;
    private bool isStart = true;
    private AnimatorStateInfo _stateInfo;
    public bool isHit;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        poly = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        _healthBar.SetMaxHealth(maxHP);
        HP = maxHP;
        _audioSource = GetComponent<AudioSource>();
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
        if (this != null) 
            SavingSystem<ComponentSnake,SnakeData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<ComponentSnake, SnakeData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        poly.enabled = data.polyEnabled;
        cap.enabled = data.capEnabled;
        _animator.SetInteger("state",data.animatorState);
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (transform.position.x > 12.7)
            transform.position = new Vector3(12.67f, transform.position.y, transform.position.z);
        if (!_stateInfo.IsName("death"))
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

        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    void Attake()
    {
        if (!_stateInfo.IsName("attake"))
        {
            if (Physics2D.OverlapCircle(attack.position, distanseAttack, catLayer))
            {
                _stateSnake = MovementState.attake;
                _animator.SetInteger("state", (int)_stateSnake);
                _catSprite.TakeDamage(takeDamage);
            }
        }
    }
    public void TakeDamage(float damage)
    {
        _writeText.snakeTarget.GetComponent<SpriteRenderer>().enabled = false;
        _writeText.W.enabled = false;
        isHit = true;
        if (!_audioSource.isPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
            _audioSource.PlayOneShot(damageClip);
        damageNow = true;
        HP -= damage;
        _healthBar.SetHealth(HP);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}