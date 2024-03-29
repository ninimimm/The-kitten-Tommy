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
    [SerializeField] private Experience XP;
    [SerializeField] public CapsuleCollider2D cap1;
    [SerializeField] public CapsuleCollider2D cap2;
    [SerializeField] public CapsuleCollider2D cap3;
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
    private SnakeData data;
    private bool isStart = true;
    private AnimatorStateInfo _stateInfo;
    public bool isHit;
    private int index = -1;
    public bool stan;

    void Start()
    {
        data = SavingSystem<ComponentSnake, SnakeData>.Load($"{gameObject.name}.data");
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _healthBar.SetMaxHealth(maxHP);
        HP = maxHP;
        _audioSource = GetComponent<AudioSource>();
        if (!MainMenu.dictSave.ContainsKey(gameObject.name))
        {
            MainMenu.dictSave.Add(gameObject.name,MainMenu.index);
            MainMenu.index ++;
        }
        if (MainMenu.isStarts[MainMenu.dictSave[gameObject.name]])
        {
            Save();
            MainMenu.isStarts[MainMenu.dictSave[gameObject.name]] = false;
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
        cap1.enabled = data.cap1Enabled;
        cap2.enabled = data.cap2Enabled;
        cap3.enabled = data.cap2Enabled;
        _animator.SetInteger("state",data.animatorState);
    }

    void Update()
    {
        _stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (transform.position.x > 12.7)
            transform.position = new Vector3(12.67f, transform.position.y, transform.position.z);
        if (!_stateInfo.IsName("death"))
        {
            if (!stan)
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
            }
            else _stateSnake = MovementState.stay;
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
            cap1.enabled = false;
            cap2.enabled = false;
            cap3.enabled = true;
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
                _catSprite.TakeDamage(takeDamage, false);
            }
        }
    }
    public void TakeDamage(float damage, bool isStan)
    {
        stan = isStan;
        if (!stan && damage > 0)
        {
            _writeText.snakeTarget.GetComponent<SpriteRenderer>().enabled = false;
            _writeText.W.enabled = false;
            isHit = true;
            if (!_audioSource.isPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
                _audioSource.PlayOneShot(damageClip);
            damageNow = true;
            HP -= damage;
            _healthBar.SetHealth(HP);
            if (HP <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death")) XP.Die();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}