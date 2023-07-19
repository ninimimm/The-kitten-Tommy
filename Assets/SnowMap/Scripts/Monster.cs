using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Monster : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject monsterAttackPrefab;
    [SerializeField] private AudioClip damageClip;
    [SerializeField] private float maxHP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Experience XP;
    [SerializeField] private int[] chanse;
    [SerializeField] private float timeToRun;
    [SerializeField] private float speed;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private Vector3 jumpVector;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private CatSprite catSprite;
    [SerializeField] private float damage;
    [SerializeField] private Image fill;
    [SerializeField] private Image fillBox;
    [SerializeField] private PolygonCollider2D poly1;
    [SerializeField] private PolygonCollider2D poly2;
    [SerializeField] private PolygonCollider2D poly3;
    private float HP;
    private float timer;
    private float number;
    public enum MovementState { idle, run1, death2, death1, attack2, attack1, run2}
    public static MovementState _stateSlime;

    private bool stan;
    private bool damageNow;
    private bool isJump;
    private bool attack;

    private AudioSource _audioSource;
    private Animator _animator;
    private Rigidbody2D _rigidbody2D;
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
    }

    // Update is called once per frame
    void Update()
    {
        if (HP > 0 && !stan)
        {
            if (timer <= 0)
            {
                timer = timeToRun;
                var rnd = new Random();
                number = rnd.Next(0, 80);
                isJump = false;
                attack = false;
            }
            else timer -= Time.deltaTime;
            if (number <= chanse[0])
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position += speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.run1);
            }
            else if (number > chanse[0] && number <= chanse[1])
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position -= speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.run1);
            }
            else if (number > chanse[1] && number <= chanse[2])
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position += speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.run2);
            }
            else if (number > chanse[2] && number <= chanse[3])
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position -= speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.run2);
            }
            else if (number > chanse[3] && number <= chanse[4] && !attack)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                attack = true;
                _animator.SetInteger("state", (int)MovementState.attack1);
            }
            else if (number > chanse[4] && number <= chanse[5] && !attack)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                attack = true;
                _animator.SetInteger("state", (int)MovementState.attack1);
            }
            else if (number > chanse[5] && number <= chanse[6] && !attack)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                attack = true;
                _animator.SetInteger("state", (int)MovementState.attack2);
            }
            else if (number > chanse[6] && number <= chanse[7] && !attack)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                attack = true;
                _animator.SetInteger("state", (int)MovementState.attack2);
            }
        }
        else
        {
            poly1.enabled = false;
            fill.enabled = false;   
            fillBox.enabled = false;
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("death1"))
                poly2.enabled = true;
            else poly3.enabled = true;
        }
    }

    public void AttackItem()
    {
        GameObject monsterAttack;
        if (transform.localScale.x > 0)
        {
            monsterAttack = Instantiate(monsterAttackPrefab, transform.position-new Vector3(0.5f,0,0), Quaternion.identity);
            monsterAttack.GetComponent<MonsterAttack>().catSprite = catSprite;
        }
        else
        {
            monsterAttack = Instantiate(monsterAttackPrefab, transform.position+new Vector3(0.5f,0,0), Quaternion.identity);
            monsterAttack.GetComponent<MonsterAttack>().catSprite = catSprite;
        }
            
    }
    public void TakeDamage(float damage, bool isStan)
    {
        stan = isStan;
        if (!stan && damage > 0)
        {
            var rnd = new Random();
            if (!_audioSource.isPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death1")
                                        && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death2"))
                _audioSource.PlayOneShot(damageClip);
            HP -= damage;
            _healthBar.SetHealth(HP);
            damageNow = true;
            if (HP <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death1")
                        && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death2"))
            {
                XP.Die();
                _animator.SetInteger("state", rnd.Next(2, 3));
            }
        }
    }
}
