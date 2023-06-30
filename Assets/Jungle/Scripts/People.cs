using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class People : MonoBehaviour,IDamageable
{
    [SerializeField] private float maxHP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private float speed;
    [SerializeField] private Transform attack1Transform;
    [SerializeField] private Transform attack2Transform;
    [SerializeField] private float distanceAttack1Transform;
    [SerializeField] private float distanceAttack2Transform;
    [SerializeField] private float idleTime;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private CatSprite _catSprite;
    [SerializeField] private float damage1;
    [SerializeField] private float damage2;
    [SerializeField] private float damage3;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private bool isFacingRight;
    public float HP;
    private bool damageNow;
    public enum MovementState { stay, walk, attack1, attack2, attack3, defense, hit, death};
    public static MovementState _statePeople;
    private Rigidbody2D rb;
    public Animator animator;
    private AnimatorStateInfo _stateInfo;
    public PolygonCollider2D _poly;
    public BoxCollider2D _boxCol;
    private float idleTimer;
    private AudioSource audioSource;
    private bool canDamage;
    private bool isDeath;
    private PeopleData data;
    private bool isStart = true;
    private int index;
    
    // Start is called before the first frame update
    void Start()
    {
        data = SavingSystem<People, PeopleData>.Load($"{gameObject.name}.data");
        _poly = GetComponent<PolygonCollider2D>();
        _boxCol = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        HP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        index = MainMenu.index;
        MainMenu.index++;
        if (MainMenu.isStarts[index])
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            Save();
            MainMenu.isStarts[index] = false;
        }
        Load();
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
        audioSource = GetComponent<AudioSource>();
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<People,PeopleData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<People, PeopleData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        _poly.enabled = data.polyEnabled;
        _boxCol.enabled = data.boxEnabled;
        animator.SetInteger("state",data.animatorState);
    }

    // Update is called once per frame
    void Update()
    {
        _stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!_stateInfo.IsName("death"))
        {
            _healthBar.SetHealth(HP);
            TryMove();
            TryIdle();
            if ((IsInLeftBound() || IsInRightBound()) && canDamage)
            {
                Random rnd = new Random();
                var number = rnd.Next(0, 6);
                canDamage = false;
                if (number == 0)
                {
                    _statePeople = MovementState.attack1;
                    animator.SetInteger("state", (int)_statePeople);
                }

                if (number == 1)
                {
                    _statePeople = MovementState.attack2;
                    animator.SetInteger("state", (int)_statePeople);
                }
                if (number == 2)
                {
                    _statePeople = MovementState.attack3;
                    animator.SetInteger("state", (int)_statePeople);
                }
            }
            if (!isDeath) animator.SetInteger("state", (int)_statePeople);
        }
        else
        {
            _boxCol.enabled = true;
            _poly.enabled = false;
            _fill.enabled = false;
            _bar.enabled = false;
        }
        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    public void Attake1()
    {
        if (Physics2D.OverlapCircle(attack1Transform.position, distanceAttack1Transform, catLayer))
            _catSprite.TakeDamage(damage1);
    }
    public void Attake2()
    {
        if (Physics2D.OverlapCircle(attack2Transform.position, distanceAttack2Transform, catLayer))
            _catSprite.TakeDamage(damage2);
    }
    public void Attake3()
    {
        if (Physics2D.OverlapCircle(attack2Transform.position, distanceAttack2Transform, catLayer))
            _catSprite.TakeDamage(damage3);
    }
    private void TryMove()
    {
        canDamage = true;
        _statePeople = MovementState.walk;
        rb.velocity = isFacingRight ? new Vector2(speed, 0) : new Vector2(-speed, 0);
    }
    private void TryIdle()
    {
        if (!(IsInLeftBound() || IsInRightBound()))
            return;
        _statePeople = MovementState.stay;
        rb.velocity = new Vector2(0, 0);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0.0f;
            Flip();
        }
    }
    private bool IsInLeftBound()
    {
        return !isFacingRight && Vector2.Distance(transform.position, leftBound.transform.position) < 0.5f;
    }

    private bool IsInRightBound()
    {
        return isFacingRight && Vector2.Distance(transform.position, rightBound.transform.position) < 0.5f;
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
    public void TakeDamage(float damage)
    {
        if(!_stateInfo.IsName("hit") && !_stateInfo.IsName("death"))
            audioSource.PlayOneShot(damageClip);
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
        if (HP <= 0)
        {
            isDeath = true;
            animator.SetInteger("state", (int)MovementState.death);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attack1Transform.position, distanceAttack1Transform);
        Gizmos.DrawWireSphere(attack2Transform.position, distanceAttack2Transform);
    }
}
