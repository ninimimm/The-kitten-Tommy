using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CatSprite : MonoBehaviour
{
    public Rigidbody2D _rb;
    private float move;
    public static MovementState _stateCat;
    private bool rotation = true;
    public Animator _animator;
    public static Animation _Animation;

    public enum MovementState
    {
        Stay,
        Run,
        jumpup,
        jumpdown,
        hit,
        damage,
        shit
    };

    private SpriteRenderer Cat;
    private Animator _snakeAnimator;
    private bool IsSnakeAttack;
    private bool IsLastSnakeAttack;
    private float HP;
    public Joystick joystick;
    [SerializeField] private float maxHP;
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public GameObject Snake;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private KnifeBar _knifeBar;
    [SerializeField] private Text _textMoney;
    [SerializeField] private Text _textHealth;
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
    [SerializeField] private int maxCountHealth;
    [SerializeField] private LayerMask checkpointLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private float slide;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float normalGravity;
    public bool isWater;
    private Vector2 vectorX;
    [SerializeField] private Vector3 spawn;
    private int countHealth;
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
    [SerializeField] public Button shitButton;
    [SerializeField] public Button hitButton;
    private bool pressedAttack;
    private bool pressedShit;
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
        countHealth = maxCountHealth;
        timerJump = timeToJump;
        audioSource = GetComponent<AudioSource>();
        _audioListener = GetComponent<AudioListener>();
        _audioListener.enabled = true;
        _textHealth.text = maxCountHealth.ToString();
        #if UNITY_ANDROID
        shitButton.gameObject.SetActive(true);
        hitButton.gameObject.SetActive(true);
        shitButton.onClick.AddListener(DoShit);
        hitButton.onClick.AddListener(DoHit);
        joystick.gameObject.SetActive(true);
        //#else
        //shitButton.gameObject.SetActive(false);
        //hitButton.gameObject.SetActive(false);
        //joystick.gameObject.SetActive(false);
        #endif
    }

    private void Update()
    {
        var lights = GetComponentsInChildren<Light2D>();
        if (transform.position.y < -1.7 && !isWater)
            foreach (var light in lights)
                light.enabled = true;
        else
            foreach (var light in lights)
                light.enabled = false;
        if (Physics2D.OverlapCircleAll(smallAttack.position, distanseSmallAttack, checkpointLayer).Length > 0 &&
            _animator.GetCurrentAnimatorStateInfo(0).IsName("shit"))
            spawn = transform.position;
        if (Fly1 != null)
        {
            if (Fly1.transform.position.x > 20 && fliesSourse.volume > 0) fliesSourse.volume -= 0.005f;
            else
                fliesSourse.volume = Math.Abs(18 - transform.position.x) < 2
                    ? Math.Abs(2 - (18 - transform.position.x)) / 2
                    : 0;
        }
        else fliesSourse.volume = 0;
        _knifeBar.SetHealth(GetComponent<Knife>().timer);
        _textMoney.text = money.ToString();
        var isIce = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, iceLayer).Length > 0;
        isWater = Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, waterLayer).Length > 0;
        // замените эту строку
        move = _animator.GetCurrentAnimatorStateInfo(0).IsName("shit") ? 0 :
        #if UNITY_ANDROID
            joystick.Horizontal;
        #elif UNITY_STANDALONE
            Input.GetAxisRaw("Horizontal");
        #endif
        if (!isIce && !isWater)
        {
            _rb.gravityScale = normalGravity;
            transform.position += new Vector3(move, 0, 0) * speed * speedMultiplier * Time.deltaTime;
        }
        else if (isWater)
        {
            _rb.gravityScale = 0.1f;
            // замените эту строку
            #if UNITY_ANDROID
                _rb.AddForce(new Vector2(joystick.Horizontal * swimSpeed * 10,joystick.Vertical * swimSpeed * 10), ForceMode2D.Force);
            #elif UNITY_STANDALONE
                _rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * swimSpeed/2.5f,Input.GetAxis("Vertical") * swimSpeed), ForceMode2D.Force);
            #endif
        }
        else
        {
            _rb.gravityScale = normalGravity;
            // замените эту строку
            #if UNITY_ANDROID
                _rb.AddForce(new Vector2(joystick.Horizontal * slide,0), ForceMode2D.Impulse);
            #elif UNITY_STANDALONE
                _rb.AddForce(new Vector2(Input.GetAxis("Horizontal") * slide,0), ForceMode2D.Impulse);
            #endif
        }
        // замените эту строку
        #if UNITY_ANDROID
            if (joystick.Vertical > 0.5)
        #elif UNITY_STANDALONE
            if (Input.GetButtonDown("Jump"))
        #endif
            timerJump = timeToJump; 

        if (timerJump > 0) timerJump -= Time.deltaTime;
        if (timerJump > 0)
        {
            if (Physics2D.OverlapCircleAll(groundCheck.position, groundCheckRadius, groundLayer).Length > 0)
            {
                jumpSourse.volume = volumeJump;
                jumpSourse.Play();
                if (!isIce)
                    _rb.velocity = Vector2.zero;
                if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("shit") && !isWater)
                    _rb.AddForce(new Vector2(_rb.velocity.x/10, jumpForce - speedMultiplier*2), ForceMode2D.Impulse);
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
    
    void DoShit() 
    {
        pressedShit = true;
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Stay") && !GetComponent<GrabbingHook>().isHooked)
        {
            _stateCat = MovementState.shit;
            _animator.SetInteger("State", (int)_stateCat);
        }
    }
    
    void DoHit()
    {
        pressedAttack = true;
        Attake();
    }
    
    private void SwitchAnimation()
    {
        #if UNITY_STANDALONE
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
        #elif UNITY_ANDROID
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
        if (pressedAttack)
        {
            _stateCat = MovementState.hit;
            pressedAttack = false;
        }
        if (damageNow)
        {
            _stateCat = MovementState.damage;
            damageNow = false;
        }
        if (pressedShit)
        {
            _stateCat = MovementState.shit;
            pressedShit = false;
        }
        _animator.SetInteger("State", (int)_stateCat);
        #endif
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
            if (countHealth <= 1)
            {
                countHealth = maxCountHealth;
                transform.position = new Vector3(1,0,0);
                SceneManager.LoadScene("FirstLevle");
            }
            else
            {
                countHealth -= 1;
                transform.position = spawn;
            }
            _textHealth.text = countHealth.ToString();
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
        }
    }
}