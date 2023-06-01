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

    public enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage, shit, death, revival };
    
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
    [SerializeField] private Transform checkpointCheck;
    [SerializeField] private float timeToJump;
    [SerializeField] private AudioSource runSourse;
    [SerializeField] private AudioSource jumpSourse;
    [SerializeField] private AudioSource damageSourse;
    [SerializeField] private AudioSource dieSourse;
    [SerializeField] private AudioSource revivalSourse;
    [SerializeField] private AudioSource swimSourse;
    [SerializeField] private AudioSource toWaterSourse;
    [SerializeField] private AudioSource fromWaterSourse;
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
    [SerializeField] public Knife knife;
    [SerializeField] private Vector3 spawn;
    [SerializeField] private Vector3 spawnRevile;
    [SerializeField] private float distanseLight;
    [SerializeField] public Sprite knifeSprite;
    [SerializeField] public Sprite poisonSprite;
    [SerializeField] private logicKnife _logicKnife;
    [SerializeField] private float distanseCheckpoint;
    [SerializeField] private AudioSource checkpointSource;
    [SerializeField] private AudioSource shitSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource phoneSource;
    [SerializeField] private AudioSource caveSource;
    
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
    private bool isDeath;
    private bool isRevive;
    public bool isPoison;
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
        isPoison = data.isPoison;
        if (isPoison) knifeSprite = poisonSprite;
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
        else if (SceneManager.GetActiveScene().name == "SecondLevle")
            transform.position = new Vector3(
                data.spawnSecondLevel[0], 
                data.spawnSecondLevel[1], 
                data.spawnSecondLevel[2]);
        else
        {
            transform.position = new Vector3(
                data.spawnThirdLevel[0], 
                data.spawnThirdLevel[1], 
                data.spawnThirdLevel[2]);
        }
    }

    private void Start()
    {
        transform.position = spawnRevile;
        if (SceneManager.GetActiveScene().name == "ThirdLevle")
        {
            normalGravity = 0.4f;
            _rb.gravityScale = 0.4f;
        }
        _logicKnife.damage = 3;
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
            knife.knife.GetComponent<SpriteRenderer>().sprite = knifeSprite;
            _healthBar.SetMaxHealth(maxHP);
            _healthBar.SetHealth(HP);   
            _textHealth.text = countHealth.ToString();
        }
    }
    
    private void Update()
    {
        if (transform.position.x > 55)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        if (stateInfo.IsName("shit")) isNowShit = true;
        else isNowShit = false;
        isGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        isCheckpoint = Physics2D.OverlapCircle(checkpointCheck.position, distanseCheckpoint, checkpointLayer);
        _knifeBar.SetHealth(knife.timer);
        UpdateLight();
        UpdateFly();
        UpdateCheckpoint();
        UpdateGround();
        UpdateJump();
        if (stateInfo.IsName("revival"))
            revivalSourse.Play();
        SwitchAnimation();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            toWaterSourse.Play();
            swimSourse.Play();
        }

        if (collision.gameObject.layer == 24)
        {
            phoneSource.Stop();
            caveSource.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            fromWaterSourse.Play();
            swimSourse.Stop();
        }
        if (collision.gameObject.layer == 24)
        {
            phoneSource.Play();
            caveSource.Stop();
        }
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
        if (isNowShit && isCheckpoint)
        {
            spawn = transform.position;
            checkpointSource.Play();
        }
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
        if (!isDeath && !isRevive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_grabbingHook.isHooked && 
                stateInfo.IsName("Stay"))
            {   
                shitSource.Play();
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
        
                if (Input.GetKeyDown(KeyCode.W) && !isWater)
                    Attake();

                if (damageNow)
                {
                    _stateCat = MovementState.damage;
                    damageNow = false;
                }
                _animator.SetInteger("State", (int)_stateCat);
            }
        }
        else if (isDeath)
        {
            _stateCat = MovementState.death;
            _animator.SetInteger("State", (int)MovementState.death);
        }
    }

    public void Revive() => transform.position = spawnRevile;
    public void ReloadScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    void Attake()
    {
        if (_stateCat == MovementState.damage) return;
        if (!stateInfo.IsName("hit"))
        {
            hitSource.Play();
            _stateCat = MovementState.hit;
            hitEnemies = Physics2D.OverlapCircleAll(smallAttack.position, distanseSmallAttack, enemyLayers);
            foreach (var enemy in hitEnemies)
                enemy.GetComponent<IDamageable>()?.TakeDamage(takeDamage);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(smallAttack.position,distanseSmallAttack);
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
        Gizmos.DrawWireSphere(checkpointCheck.position,distanseCheckpoint);
    }
    public void TakeDamage(float damage)
    {
        damageSourse.volume = volumeDamage;
        damageSourse.Play();
        HP -= damage;
        _healthBar.SetHealth(HP);
        
        if (HP <= 0)
        {
            _grabbingHook.line.enabled = false;
            _grabbingHook.isHooked = false;
            _grabbingHook._joint2D.enabled = false;
            if (countHealth <= 1)
            {
                isDeath = true;
                dieSourse.Play();
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
        else damageNow = true;
    }
}