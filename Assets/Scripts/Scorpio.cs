using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cкорпионы патрулирует свою ограниченную область (слева направо ходят)
// если кот встречается на пути (кот приблизился на attackRange),
// то скорпионы поворачиваются к нему (если надо) и атакуют, пока кот рядом
// при этом дальше скорпионы идут в том направлении в котором были после атаки
// скорпионы не мешают друг другу ходить, могут ходить “сквозь“ друг друга
public class Scorpio : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject cat;
    [SerializeField] private GameObject leftBound;
    [SerializeField] private GameObject rightBound;
    [SerializeField] private float health = 20;
    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float damage = 0.1f; 
    [SerializeField] private float attackRange = 0.5f; 
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attack;
    [SerializeField] private float idleTime = 5.0f;
    private float idleTimer = 0.0f;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isDead = false;
    private bool isFacingRight = false;
    private CatSprite catScript;  

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        catScript = cat.GetComponent<CatSprite>();
    }

    private void Update()
    {
        Attack();
        if (IsInLeftBound() || IsInRightBound())
            Idle();
        else
            Move();
    }

    private void Move()
    {
        animator.SetBool("moving", true);
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
    
    private void Idle()
    {
        rb.velocity = new Vector2(0, 0);
        animator.SetBool("moving", false);
        idleTimer += Time.deltaTime;
        if (idleTimer >= idleTime)
        {
            idleTimer = 0.0f;
            Flip();
        }
    }

    private bool IsCloseToCat()
    {
        return Vector2.Distance(transform.position, cat.transform.position) < attackRange;
    }

    public void TakeDamage(float damage)
    {
        if (!isDead)
        {
            health -= damage;
            if (health <= 0)
            {
                isDead = true;
                animator.SetTrigger("die");
                Die();
            }
            else
            {
                animator.SetTrigger("hurt");
            }
        }
    }

    private void Die()
    {
        isDead = true;
        health = 10;
        transform.position = new Vector3(1, 0, 0);
    }

    private void Attack()
    {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("scorpioattack")) 
        {
            var hitCat = Physics2D.OverlapCircleAll(attack.position, attackRange, catLayer);
            if (hitCat.Length > 0)
            {
                animator.SetBool("moving", false);
                animator.SetTrigger("scorpioattack");
            }

            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);
        }
    }

    private bool IsCatBehindScorpion()
    {
        if (isFacingRight)
        {
            return cat.transform.position.x < transform.position.x;
        }
        
        return cat.transform.position.x > transform.position.x;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(Vector3.up, 180.0f, Space.World);
    }
}

   