using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CatSprite : MonoBehaviour
{
    public Rigidbody2D _rb;
    private float move;
    public static MovementState _stateCat;
    private bool rotation = true;
    public  Animator _animator;
    public static Animation _Animation;
    public enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage, shit };
    private SpriteRenderer Cat;
    private Animator _snakeAnimator;
    private bool IsSnakeAttack;
    private bool IsLastSnakeAttack;
    private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public GameObject Snake;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private KnifeBar _knifeBar;
    [SerializeField] private Text _text;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float timeToJump;
    [SerializeField] private AudioSource runSourse;
    [SerializeField] private AudioSource jumpSourse;
    [SerializeField] private AudioSource damageSourse;
    [SerializeField] private AudioClip audioFly;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioSource fliesSourse;
    [SerializeField] private GameObject Fly1;
    private AudioSource audioSource;
    private float timerJump;
    private PolygonCollider2D _poly;
    public Transform smallAttack;
    public float distanseSmallAttack = 0.2f;
    public LayerMask enemyLayers;
    public int takeDamage = 1;
    private float speedMultiplier = 1f;
    private bool damageNow;
    private AudioListener _audioListener;
    [Range(0, 1f)] public float volumeRun;
    [Range(0, 1f)] public float volumeJump;
    [Range(0, 1f)] public float volumeDamage;
    [Range(0, 1f)] public float volumeFly;

    public int money = 0;

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }


    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _poly = GetComponent<PolygonCollider2D>();
        Cat = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _snakeAnimator = Snake.GetComponent<Animator>();
        _Animation = GetComponent<Animation>();
        transform.Rotate(0f,180f,0f);
        _healthBar.SetMaxHealth(maxHP);
        _knifeBar.SetMaxHealth(GetComponent<Knife>().attackIntervale);
        HP = maxHP;
        timerJump = timeToJump;
        audioSource = GetComponent<AudioSource>();
        _audioListener = GetComponent<AudioListener>();
        _audioListener.enabled = true;
    }

    private void Update()
    {
        if (Fly1 != null)
            fliesSourse.volume = Math.Abs(18 - transform.position.x) < 2
                ? Math.Abs(2 - (18 - transform.position.x)) / 2
                : 0;
        else fliesSourse.volume = 0;
        _knifeBar.SetHealth(GetComponent<Knife>().timer);
        _text.text = money.ToString();
        move = Input.GetAxisRaw("Horizontal");
        transform.position += new Vector3(move, 0, 0) * speed * speedMultiplier * Time.deltaTime;
        if (Input.GetButtonDown("Jump"))
            timerJump = timeToJump;

        if (timerJump > 0) timerJump -= Time.deltaTime;
        if (timerJump > 0)
        {
            if (Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer).Length > 0)
            {
                jumpSourse.volume = volumeJump;
                jumpSourse.Play();
                _rb.velocity = Vector2.zero;
                _rb.AddForce(new Vector2(0, jumpForce - speedMultiplier*2), ForceMode2D.Impulse);
                timerJump = 0;
            }
        }

        if (move != 0 && Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer).Length > 0)
        {
            if (!runSourse.isPlaying)
            {
                runSourse.volume = volumeRun;
                runSourse.Play();
            }
        }
        SwitchAnimation();
    }

    private void SwitchAnimation()
    {
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Stay") && Input.GetKeyDown(KeyCode.CapsLock)
            && !GetComponent<GrabbingHook>().isHooked)
        {
            _stateCat = MovementState.shit;
            _animator.SetInteger("State", (int)_stateCat);
        }
        else
        {
            if (move > 0)
            {
                _stateCat = MovementState.Run;
                if (rotation)
                {
                    transform.Rotate(0f,180f,0f);
                    rotation = false;
                }
            }
            else if (move < 0)
            {
                _stateCat = MovementState.Run;
                if (!rotation)
                {
                    transform.Rotate(0f,180f,0f);
                    rotation = true;
                }
            }
            else
                _stateCat = MovementState.Stay;

            if (_rb.velocity.y > 1f) _stateCat = MovementState.jumpup;
            else if (_rb.velocity.y < - 1f) _stateCat = MovementState.jumpdown;
        
            if (Input.GetKeyDown(KeyCode.Q))
                Attake();

            if (damageNow)
            {
                _stateCat = MovementState.damage;
                damageNow = false;
            }
            _animator.SetInteger("State", (int)_stateCat);
        }
    }

    void Attake()
    {
        if (_stateCat == MovementState.damage) return;
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("hit"))
        {
            _stateCat = MovementState.hit;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(smallAttack.position, distanseSmallAttack, enemyLayers);
            foreach (var enemy in hitEnemies)
            {
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(takeDamage);
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (smallAttack.position == null)
            return;
        Gizmos.DrawWireSphere(smallAttack.position,distanseSmallAttack);
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
    }
    public void TakeDamage(float damage)
    {
        damageSourse.volume = volumeDamage;
        damageSourse.Play();
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
        if (HP <= 0)
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            transform.position = new Vector3(1, 0, 0);
        }
            
    }
}