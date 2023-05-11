using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float damage; 
    [SerializeField] private float attackRange; 
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attackCircle;
    [SerializeField] private float idleTime;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    [SerializeField] private AudioClip runSound;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    private float idleTimer = 0.0f;
    private bool damageNow = false;
    private BoxCollider2D boxCollider;
    private PolygonCollider2D polygonCollider;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = false;
    private MovementState stateScorpio;
    private enum MovementState { idle, walk, attack, death, hurt };

    private void Start()
    {
        health = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        polygonCollider = GetComponent<PolygonCollider2D>();
        polygonCollider.enabled = false;
        boxCollider.enabled = true;
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
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
    }

    private void TryChangeOnDamageState()
    {
        if (damageNow && health > 0)
        {
            stateScorpio = MovementState.hurt;
            damageNow = false;
        }
        else if (health <= 0)
            stateScorpio = MovementState.death;
    }

    private void TryMove()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(runSound);
        }
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
        health -= damage;
        _healthBar.SetHealth(health);
        damageNow = true;
    }

    private void TryAttack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("ScorpioAttack")) 
        {
            var hitCat = Physics2D.OverlapCircleAll(attackCircle.position, attackRange, catLayer);
            if (hitCat.Length > 0)
                stateScorpio = MovementState.attack;
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

   