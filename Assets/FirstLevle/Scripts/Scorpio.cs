using System;
using UnityEngine;
using UnityEngine.UI;

// cкорпионы патрулирует свою ограниченную область (слева направо ходят)
// если кот встречается на пути (есть коллизия с AttackCircle),
// то скорпионы атакуют, пока кот рядом (есть коллизия с AttackCircle)
// скорпионы не мешают друг другу ходить, могут ходить “сквозь“ друг друга
public class Scorpio : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private float maxHP;
    [SerializeField] public float HP;
    [SerializeField] private float speed;
    [SerializeField] private float damage; 
    [SerializeField] private float attackRange; 
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attackCircle;
    [SerializeField] private float idleTime;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private float distanseRunSourse;
    [SerializeField] private Transform catTransform;
    private AudioSource audioSource;
    private float idleTimer = 0.0f;
    private bool damageNow = false;
    public BoxCollider2D boxCollider;
    public PolygonCollider2D polygonCollider;
    public Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private MovementState stateScorpio;
    private enum MovementState { idle, walk, attack, death, hurt };
    private ScorpioData data;
    private bool isStart = true;

    private void Start()
    {
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        boxCollider.enabled = true;
        audioSource = GetComponent<AudioSource>();
        if (!ScorpioData.start.Contains(gameObject.name))
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            Save();
            ScorpioData.start.Add(gameObject.name);
        }
        Load();
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
    }

    public void Save()
    {
        SavingSystem<Scorpio,ScorpioData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Scorpio, ScorpioData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        polygonCollider.enabled = data.polyEnabled;
        boxCollider.enabled = data.boxEnabled;
        animator.SetInteger("stateScorpio",data.animatorState);
    }
    
    private void Update()
    {
        audioSource.volume = Math.Abs(catTransform.position.x-transform.position.x) < distanseRunSourse ?
            Math.Abs(catTransform.position.x-transform.position.x)/distanseRunSourse + 0.2f : 0;
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ScorpioDeath"))
        {
            TryMove();
            TryIdle();
            TryAttack();
            TryChangeOnDamageState();
            animator.SetInteger("stateScorpio", (int)stateScorpio);
        }
        else
        {
            polygonCollider.enabled = true;
            boxCollider.enabled = false;
            _fill.enabled = false;
            _bar.enabled = false;
        }
        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    private void TryChangeOnDamageState()
    {
        if (damageNow && HP > 0)
        {
            stateScorpio = MovementState.hurt;
            damageNow = false;
        }
        else if (HP <= 0)
            stateScorpio = MovementState.death;
    }

    private void TryMove()
    {
        if (!audioSource.isPlaying)
            audioSource.PlayOneShot(runClip);
        stateScorpio = MovementState.walk;
        if (isFacingRight)
            rb.velocity = new Vector2(speed, 0);
        else
            rb.velocity = new Vector2(-speed, 0);
    }
    private bool IsInLeftBound()
    {
        return !isFacingRight && Vector2.Distance(transform.position, leftBound.transform.position) < 0.5f;
    }

    private bool IsInRightBound()
    {
        return isFacingRight && Vector2.Distance(transform.position, rightBound.transform.position) < 0.5f;
    }
    
    private void TryIdle()
    {
        if (!(IsInLeftBound() || IsInRightBound()))
            return;
        stateScorpio = MovementState.idle;
        rb.velocity = new Vector2(0, 0);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0.0f;
            Flip();
        }
    }

    public void TakeDamage(float damage)
    {
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
    }

    private void TryAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ScorpioAttack")) 
        {
            var hitCat = Physics2D.OverlapCircleAll(attackCircle.position, attackRange, catLayer);
            if (hitCat.Length > 0)
            {
                stateScorpio = MovementState.attack;
                animator.SetInteger("stateScorpio", (int)stateScorpio);
            }
                
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
    
    private void OnDrawGizmosSelected()
    {
        if (attackCircle.position == null)
            return;
        Gizmos.DrawWireSphere(attackCircle.position, attackRange);
    }
}

   