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
    [SerializeField] private Knife knife;
    [SerializeField] private Vector3 spawn;
    [SerializeField] private Vector3 spawnRevile;
    [SerializeField] private float distanseLight;
    [SerializeField] private Sprite knifeSprite;
    [SerializeField] private logicKnife _logicKnife;
    [SerializeField] private float distanseCheckpoint;
    
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
        transform.position = spawnRevile;
        if (SceneManager.GetActiveScene().name == "ThirdLevle")
        {
            normalGravity = 0.4f;
            _rb.gravityScale = 0.4f;
        }
        knife.knife.GetComponent<SpriteRenderer>().sprite = knifeSprite;
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
        UpdateWaterSounds();
        if (stateInfo.IsName("revival"))
            revivalSourse.Play();
        SwitchAnimation();
    }

    private void UpdateWaterSounds()
    {
        var yCoord = transform.position.y;
        var isOnTop = yCoord > -1.4 && transform.position.x < 30;
        if (!isOnTop)
            isOnTop = yCoord > 0.6 && transform.position.x > 30;
        var isOnBound = transform.position.x < 27.6 && yCoord < -1.3 && yCoord > -1.6;
        if (!isOnBound)
            isOnBound = transform.position.x > 31.53 && yCoord < 0.6 && yCoord > 0.36;
        var isCollidingToWater = (bool)Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, waterLayer);
        if (!isCollidingToWater)
            return;
        if (!fromWaterSourse.isPlaying && Input.GetAxis("Vertical") > 0 && isOnBound && !isOnTop) 
            fromWaterSourse.Play();
        else if (!toWaterSourse.isPlaying && Input.GetAxis("Vertical") <= 0 && isOnBound && !isOnTop)
            toWaterSourse.Play();
        else if (!swimSourse.isPlaying && (Math.Abs(moveInWater) > 0.3 || Math.Abs(Input.GetAxis("Vertical")) > 0.3) && !isOnTop)
            swimSourse.Play();
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
        if (!isDeath && !isRevive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_grabbingHook.isHooked && 
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
        
                if (Input.GetKeyDown(KeyCode.W))
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