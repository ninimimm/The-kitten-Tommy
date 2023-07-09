using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CatSprite : MonoBehaviour
{
    public Rigidbody2D _rb;
    public float move;
    public static MovementState _stateCat;
    public Animator _animator;

    public enum MovementState { Stay, Run, jumpup, jumpdown, hit, damage, shit, death, revival };
    
    public float HP;
    public float XP;
    [SerializeField] public float maxHP;
    [SerializeField] public float mapXP;
    [SerializeField] public float speed = 4.0f;
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public HealthBar _healthBar;
    [SerializeField] public HealthBar greenBar;
    [SerializeField] public KnifeBar _knifeBar;
    [SerializeField] public Text _textMoney;
    [SerializeField] public Text _textHealth;
    [SerializeField] public Transform groundCheck1;
    [SerializeField] public Transform groundCheck2;
    [SerializeField] public float groundCheckRadius;
    [SerializeField] public LayerMask groundLayer;
    [SerializeField] public Transform checkpointCheck;
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
    [SerializeField] public LayerMask checkpointLayer;
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
    [SerializeField] public float distanseCheckpoint;
    [SerializeField] private AudioSource checkpointSource;
    [SerializeField] private AudioSource shitSource;
    [SerializeField] private AudioSource hitSource;
    [SerializeField] private AudioSource phoneSource;
    [SerializeField] private AudioSource caveSource;
    [SerializeField] private LayerMask crateLayer;
    [SerializeField] private SpriteRenderer spriteShiftSand;
    [SerializeField] private TextMeshProUGUI textShiftSand;
    [SerializeField] private SpriteRenderer spriteShiftBoss;
    [SerializeField] private TextMeshProUGUI textShiftBoss;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private GameObject windPrefab;
    
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
    public bool isGround;
    private bool isCheckpoint;
    private Collider2D[] hitEnemies;
    private float moveInWater;
    private AnimatorStateInfo stateInfo;
    private bool isDeath;
    private bool isRevive;
    public bool isPoison;
    public bool isOnWall;
    [Range(0, 1f)] public float volumeRun;
    [Range(0, 1f)] public float volumeJump;
    [Range(0, 1f)] public float volumeDamage;
    
    public int money;
    private CatData data;
    public bool canTakeDamage;
    private bool isJumpOnWall;
    public bool isOnRight;
    public bool isOnLeft;
    public bool isInCave;
    private float currentY;
    private int index = -1;
    public bool canSpawn;
    public bool inMiniMap;

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = multiplier;
    }

    
    public void Save()
    {
        if (this != null) 
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
                data.spawnFirstLevel[0]+1f, 
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
        spawnRevile = transform.position;
        spawn = transform.position;
        canSpawn = data.canSpawn;
    }

    private void Start()
    {
        data = SavingSystem<CatSprite, CatData>.Load($"{gameObject.name}.data");
        if (SceneManager.GetActiveScene().name == "Jungle") key = "";
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
        canTakeDamage = true;
        if (index == -1)
        {
            index = MainMenu.index;
            MainMenu.index += 100;
        }
        else
        {
            index++;
        }
        if (MainMenu.isStarts[index] && SceneManager.GetActiveScene().name == "FirstLevle")
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            greenBar.SetMaxHealth(mapXP);
            countHealth = maxCountHealth;
            _textHealth.text = maxCountHealth.ToString();
            Save();
            MainMenu.isStarts[index] = false;
        }
        Load();
        _textMoney.text = money.ToString();
        knife.knife.GetComponent<SpriteRenderer>().sprite = knifeSprite;
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
        greenBar.SetMaxHealth(mapXP);
        greenBar.SetHealth(XP);
        _textHealth.text = countHealth.ToString();
    }

    private void FixedUpdate()
    {
        if (!inMiniMap)
            UpdateGround();
    }

    private void Update()
    {
        if (!inMiniMap)
        {
            if (stateInfo.IsName("revival"))
                revivalSourse.Play();
            else if (!isDeath)
            {
                if (Input.GetKeyDown(KeyCode.N))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex%5 + 1);
                if (Input.GetKeyDown(KeyCode.B))
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                if (stateInfo.IsName("shit")) isNowShit = true;
                else isNowShit = false;
                isGround = Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, groundLayer) ||
                           Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, groundLayer);
                if (isGround) isJumpOnWall = false;
                isCheckpoint = Physics2D.OverlapCircle(checkpointCheck.position, distanseCheckpoint, checkpointLayer);
                if (isCheckpoint)
                {
                    if (spriteShiftSand != null && spriteShiftBoss != null)
                    {
                        spriteShiftSand.enabled = true;
                        textShiftSand.enabled = true;
                        spriteShiftBoss.enabled = true;
                        textShiftBoss.enabled = true;
                    }
                }
                else
                {
                    if (spriteShiftSand != null && spriteShiftBoss != null)
                    {
                        spriteShiftSand.enabled = false;
                        textShiftSand.enabled = false;
                        spriteShiftBoss.enabled = false;
                        textShiftBoss.enabled = false;
                    }
                }
                _knifeBar.SetHealth(knife.timer);
                UpdateLight();
                UpdateFly();
                UpdateCheckpoint();
                UpdateWall();
                if (isInCave)
                    FreezeYInTree();
                else
                    FreezeYPutTree();
                UpdateJump();
            }
            SwitchAnimation();  
        }
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
            isInCave = true;
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
            isInCave = false;
        }
    }

    private void UpdateWall()
    {
        if (Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, wallLayer) || 
            Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, wallLayer) &&
            !isJumpOnWall)
        {
            _rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }
        else
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.freezeRotation = true;
            isOnWall = false;
            isOnRight = false;
            isOnLeft = false;
        }
            
        if (!isOnWall &&Physics2D.OverlapCircle(smallAttack.position, distanseSmallAttack*1.5f, wallLayer))
        {
            currentY = (transform.position - new Vector3(0, 0.2f, 0)).y;
            if (transform.rotation.y == 0)
            {
                isOnRight = true;
                transform.eulerAngles = new Vector3(0,0,90);
            }
            else
            {
                isOnLeft = true;
                transform.eulerAngles = new Vector3(180,0,-90);
            }
            isOnWall = true;
            isJumpOnWall = false;
        }

        if (isOnWall &&
            !(Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, wallLayer) ||
              Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, wallLayer)) &&
            !isJumpOnWall)
        {
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.freezeRotation = true;
            isOnWall = false;
            currentY = 10000;
            if (isOnRight)
                isOnRight = false;
            else if (isOnLeft)
                isOnLeft = false;
        }
    }

    private void FreezeYInTree()
    {
        if (isOnWall)
        {
            if (currentY < 6.3)
            {
                if (transform.position.y > 6.3)
                    transform.position = new Vector3(transform.position.x, 6.3f, transform.position.z);
                else if (transform.position.y < 4.9f)
                    transform.position = new Vector3(transform.position.x, 4.9f, transform.position.z);
            }
            else if (currentY >= 6.4 && currentY < 8.4)
            {
                if (transform.position.y > 8.2f)
                    transform.position = new Vector3(transform.position.x, 8.2f, transform.position.z);
                else if (transform.position.y < 6.9)
                    transform.position = new Vector3(transform.position.x, 6.9f, transform.position.z);
            }
            else if (currentY >= 8.9 && currentY < 10.9)
            {
                if (transform.position.y > 10.3)
                    transform.position = new Vector3(transform.position.x, 10.3f, transform.position.z);
                else if (transform.position.y < 8.9f)
                    transform.position = new Vector3(transform.position.x, 8.9f, transform.position.z);
            }
        }
    }

    private void FreezeYPutTree()
    {
        if (isOnWall)
        {
            if (transform.position.x < 56)
            {
                if (currentY < 18.2)
                {
                    if (transform.position.y > 18.2)
                        transform.position = new Vector3(transform.position.x, 18.2f, transform.position.z);
                    else if (transform.position.y < 1.3f)
                        transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
                }
            }
            else if (transform.position.x > 56)
            {
                if (currentY < 4.2)
                {
                    if (transform.position.y > 4.2)
                        transform.position = new Vector3(transform.position.x, 4.2f, transform.position.z);
                    else if (transform.position.y < 1.3f)
                        transform.position = new Vector3(transform.position.x, 1.3f, transform.position.z);
                }
                else if (currentY < 18.2)
                {
                    if (transform.position.y > 18.2)
                        transform.position = new Vector3(transform.position.x, 18.2f, transform.position.z);
                    else if (transform.position.y < 6f)
                        transform.position = new Vector3(transform.position.x, 6f, transform.position.z);
                }
            }
        }
    }
    
    private void UpdateLight()
    {
        if (currentScene == "SecondLevle" && isInCave)
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
        move = isNowShit || _grabbingHook.isHookedStatic || isJumpOnWall ? 0 : isOnWall? Input.GetAxisRaw("Vertical") : Input.GetAxisRaw("Horizontal");
        moveInWater = isNowShit ? 0 : Input.GetAxis("Horizontal");
        isIce = Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, iceLayer) ||
                Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, iceLayer);
        isWater = Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, waterLayer) ||
                  Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, waterLayer);
        if (!isIce && !isWater && !isOnWall)
        {
            _rb.gravityScale = normalGravity;
            transform.position += speed * speedMultiplier * Time.deltaTime * new Vector3(move, 0, 0) ;
        }
        else if (isWater)
        {
            _rb.gravityScale = 0.1f;
            _rb.AddForce(new Vector2(moveInWater * swimSpeed/2.5f,Input.GetAxis("Vertical") * swimSpeed), ForceMode2D.Force);
        }
        else if (isIce)
        {
            _rb.gravityScale = normalGravity;
            _rb.AddForce(new Vector2(moveInWater * slide,0), ForceMode2D.Force);
        }
        else if (isOnWall)
        {
            transform.position += speed * speedMultiplier * Time.deltaTime * new Vector3(0, move, 0) ;
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
        if (isOnWall && Input.GetButtonDown("Jump"))
        {
            move = 0;
            _rb.constraints = RigidbodyConstraints2D.None;
            _rb.freezeRotation = true;
            if (isOnRight)
            {
                if (transform.rotation.y == 0)
                    _rb.AddForce(new Vector2(-jumpForce/3,jumpForce), ForceMode2D.Impulse);
                else
                    _rb.AddForce(new Vector2(-jumpForce/1.5f,0), ForceMode2D.Impulse);
                transform.eulerAngles = new Vector3(0, 180, 0);
                isOnRight = false;
            }
            else if (isOnLeft)
            {
                if (transform.rotation.y == 0)
                    _rb.AddForce(new Vector2(jumpForce/1.5f,0), ForceMode2D.Impulse);
                else
                    _rb.AddForce(new Vector2(jumpForce/3,jumpForce), ForceMode2D.Impulse);
                transform.eulerAngles = new Vector3(0,0,0);
                isOnLeft = false;
            }
            isJumpOnWall = true;
            isOnRight = false;
            isOnLeft = false;
        }
        else
        {
            if (Input.GetButtonDown("Jump")) timerJump = timeToJump;
            if (timerJump > 0) timerJump -= Time.deltaTime;
            if (timerJump > 0)
            {
                if (isGround && 
                    !(_grabbingHook.isHookedDynamic && 
                      (Physics2D.OverlapCircle(groundCheck1.position, groundCheckRadius, crateLayer) ||
                       Physics2D.OverlapCircle(groundCheck2.position, groundCheckRadius, waterLayer))))
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
    }

    private void SwitchAnimation()
    {
        if (!isDeath && !isRevive)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_grabbingHook.isHookedStatic && !_grabbingHook.isHookedDynamic && 
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
                    if (isOnRight)
                        transform.eulerAngles = new Vector3(0,0,90);
                    else if (isOnLeft)
                        transform.eulerAngles = new Vector3(0,180,90);
                    else
                    {
                        _rb.freezeRotation = false;
                        transform.eulerAngles = new Vector3(0, 0, 0);
                        _rb.freezeRotation = true;
                    }

                }
                else if (move < 0)
                {
                    _stateCat = MovementState.Run;
                    if (isOnRight)
                        transform.eulerAngles = new Vector3(0,180,-90);
                    else if (isOnLeft)
                        transform.eulerAngles = new Vector3(0, 0, -90);
                    else
                    {
                        _rb.freezeRotation = false;
                        transform.eulerAngles = new Vector3(0, -180, 0);
                        _rb.freezeRotation = true; 
                    }
                }
                else
                    _stateCat = MovementState.Stay;

                if (_rb.velocity.y > 1f) _stateCat = MovementState.jumpup;
                else if (_rb.velocity.y < - 1f) _stateCat = MovementState.jumpdown;
        
                if (Input.GetKeyDown(KeyCode.W) && !isWater && !isOnWall)
                    Attake();
                else if (Input.GetKeyDown(KeyCode.S) && !isWater && !isOnWall)
                    Wind();

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
                enemy.GetComponent<IDamageable>()?.TakeDamage(takeDamage, false);
        }
    }

    void Wind()
    {
        if (XP > mapXP / 5)
        {
            XP -= mapXP / 5;
            greenBar.SetHealth(XP);
            var wind = Instantiate(windPrefab, transform.position + new Vector3(1, -0.3f, 0), Quaternion.identity);
            wind.transform.localScale = transform.rotation.y == 0 ? 
                wind.transform.localScale : 
                new Vector3(-wind.transform.localScale.x,wind.transform.localScale.y,wind.transform.localScale.z);
            wind.GetComponent<Wind>().isRight = transform.rotation.y == 0;
            wind.transform.position = transform.rotation.y == 0
                ? wind.transform.position
                : wind.transform.position - new Vector3(2, 0, 0);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(smallAttack.position,distanseSmallAttack);
        Gizmos.DrawWireSphere(groundCheck1.position,groundCheckRadius);
        Gizmos.DrawWireSphere(groundCheck2.position,groundCheckRadius);
        Gizmos.DrawWireSphere(checkpointCheck.position,distanseCheckpoint);
    }
    public void TakeDamage(float damage, bool isStan)
    {
        if (canTakeDamage)
        {
            damageSourse.volume = volumeDamage;
            damageSourse.Play();
            HP -= damage;
            _healthBar.SetHealth(HP);
        
            if (HP <= 0)
            {
                isOnWall = false;
                isOnRight = false;
                isOnLeft = false;
                _grabbingHook.line.enabled = false;
                _grabbingHook.isHookedStatic = false;
                _grabbingHook.isHookedDynamic = false;
                _grabbingHook._joint2DDynamic.enabled = false;
                if (countHealth <= 1)
                {
                    if (!isDeath) dieSourse.Play();
                    isDeath = true;
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
}