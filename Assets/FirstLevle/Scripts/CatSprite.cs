using System;
using System.Linq;
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

    public enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage, shit };
    
    public float HP;
    [SerializeField] public float maxHP;
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public HealthBar _healthBar;
    [SerializeField] public KnifeBar _knifeBar;
    [SerializeField] public Text _textMoney;
    [SerializeField] public Text _textHealth;
    [SerializeField] public Transform groundCheck;
    [SerializeField] public float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float timeToJump;
    [SerializeField] private AudioSource runSourse;
    [SerializeField] private AudioSource jumpSourse;
    [SerializeField] private AudioSource damageSourse;
    [SerializeField] private AudioSource fliesSourse;
    [SerializeField] private GameObject Fly1;
    [SerializeField] private int maxCountHealth;
    [SerializeField] private LayerMask checkpointLayer;
    [SerializeField] private LayerMask iceLayer;
    [SerializeField] private float slide;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float normalGravity;
    [SerializeField] public Light2D[] lights;
    [SerializeField] private Knife knife;
    [SerializeField] private Vector3 spawn;
    [SerializeField] private float distanseLight;
    public bool isWater;
    public int countHealth;
    private float timerJump;
    public Transform smallAttack;
    public float distanseSmallAttack = 0.2f;
    public LayerMask enemyLayers;
    public int takeDamage = 1;
    private float speedMultiplier = 1f;
    private bool damageNow;
    private AudioListener _audioListener;
    private bool pressedAttack;
    private bool pressedShit;
    public string key;
    public bool isBossDead;
    private GrabbingHook _grabbingHook;
    private string currentScene;
    private bool isIce;
    private bool isNowShit;
    private bool isGround;
    private bool isCheckpoint;
    private Collider2D[] hitEnemies;
    private float moveInWater;
    private AnimatorStateInfo stateInfo;
    [Range(0, 1f)] public float volumeRun;
    [Range(0, 1f)] public float volumeJump;
    [Range(0, 1f)] public float volumeDamage;
    
    public int money;
    private CatData data;

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    
    public void Save()
    {
        SavingSystem<CatSprite,CatData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        data = SavingSystem<CatSprite, CatData>.Load($"{gameObject.name}.data");
        HP = data.HP;
        countHealth = data.countHealth;
        takeDamage = data.takeDamage;
        key = data.key;
        money = data.money;
        if (SceneManager.GetActiveScene().name == "FirstLevle")
            transform.position = new Vector3(
                data.spawnFirstLevel[0], 
                data.spawnFirstLevel[1], 
                data.spawnFirstLevel[2]);
        else
            transform.position = new Vector3(
                data.spawnSecondLevel[0], 
                data.spawnSecondLevel[1], 
                data.spawnSecondLevel[2]);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        transform.Rotate(0f,180f,0f);
        _knifeBar.SetMaxHealth(knife.attackIntervale);
        timerJump = timeToJump;
        _audioListener = GetComponent<AudioListener>();
        _audioListener.enabled = true;
        _grabbingHook = GetComponent<GrabbingHook>();
        currentScene = SceneManager.GetActiveScene().name;
        if (currentScene != "Training")
        {
            if (!CatData.start.Contains(gameObject.name))
            {
                HP = maxHP;
                _healthBar.SetMaxHealth(maxHP);
                countHealth = maxCountHealth;
                _textHealth.text = maxCountHealth.ToString();
                Save();
                CatData.start.Add(gameObject.name);
            }
            Load();
            _healthBar.SetMaxHealth(maxHP);
            _healthBar.SetHealth(HP);   
            _textHealth.text = countHealth.ToString();
        }
    }
    
    private void Update()
    {
        stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("shit")) isNowShit = true;
        else isNowShit = false;
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isCheckpoint = Physics2D.OverlapCircle(smallAttack.position, distanseSmallAttack, checkpointLayer);
        _knifeBar.SetHealth(knife.timer);
        UpdateLight();
        UpdateFly();
        UpdateCheckpoint();
        UpdateGround();
        UpdateJump();
        SwitchAnimation();
    }

    private void UpdateLight()
    {
        if (currentScene == "SecondLevle")
        {
            foreach (var light in lights)
            {
                if ((light.transform.position - transform.position).sqrMagnitude < distanseLight * distanseLight)
                {
                    light.enabled = true;
                    light.intensity = (distanseLight-Vector3.Distance(light.transform.position, transform.position)) / distanseLight;
                }
                else light.enabled = false;
            }
        } 
    }

    private void UpdateFly()
    {
        if (Fly1 != null)
        {
            if (Fly1.transform.position.x > 20 && fliesSourse.volume > 0) fliesSourse.volume -= 0.005f;
            else
                fliesSourse.volume = Math.Abs(18 - transform.position.x) < 2
                    ? Math.Abs(2 - (18 - transform.position.x)) / 2
                    : 0;
        }
        else fliesSourse.volume = 0;
    }

    private void UpdateCheckpoint()
    {
        if (isNowShit && isCheckpoint) spawn = transform.position;
    }

    private void UpdateGround()
    {
        move = isNowShit ? 0 : Input.GetAxisRaw("Horizontal");
        moveInWater = isNowShit ? 0 : Input.GetAxis("Horizontal");
        isIce = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, iceLayer);
        isWater = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, waterLayer);
        if (!isIce && !isWater)
        {
            _rb.gravityScale = normalGravity;
            transform.position += speed * speedMultiplier * Time.deltaTime * new Vector3(move, 0, 0) ;
        }
        else if (isWater)
        {
            _rb.gravityScale = 0.1f;
            _rb.AddForce(new Vector2(moveInWater * swimSpeed/2.5f,Input.GetAxis("Vertical") * swimSpeed), ForceMode2D.Force);
        }
        else
        {
            _rb.gravityScale = normalGravity;
            _rb.AddForce(new Vector2(moveInWater * slide,0), ForceMode2D.Impulse);
        }
        if (move != 0 && isGround)
        {
            if (!runSourse.isPlaying)
            {
                runSourse.volume = volumeRun;
                runSourse.Play();
            }
        }
    }

    private void UpdateJump()
    {
        if (Input.GetButtonDown("Jump")) timerJump = timeToJump;
        if (timerJump > 0) timerJump -= Time.deltaTime;
        if (timerJump > 0)
        {
            if (isGround)
            {
                jumpSourse.volume = volumeJump;
                jumpSourse.Play();
                if (!isIce)
                    _rb.velocity = Vector2.zero;
                if (!isNowShit && !isWater)
                    _rb.AddForce(new Vector2(_rb.velocity.x/10, jumpForce - speedMultiplier*2), ForceMode2D.Impulse);
                timerJump = 0;
            }
        }
    }

    private void SwitchAnimation()
    {
        if (Input.GetKeyDown(KeyCode.CapsLock) && !_grabbingHook.isHooked && 
            stateInfo.IsName("Stay"))
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
        if (!stateInfo.IsName("hit"))
        {
            _stateCat = MovementState.hit;
            hitEnemies = Physics2D.OverlapCircleAll(smallAttack.position, distanseSmallAttack, enemyLayers);
            foreach (var enemy in hitEnemies)
                enemy.GetComponent<IDamageable>()?.TakeDamage(takeDamage);
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
            _grabbingHook.line.enabled = false;
            _grabbingHook.isHooked = false;
            _grabbingHook._joint2D.enabled = false;
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