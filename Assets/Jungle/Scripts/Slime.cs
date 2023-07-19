using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Slime : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject slimeAttackPrefab;
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
    private float HP;
    private float timer;
    private float number;
    public enum MovementState { stay, walk, jump, damage, death }
    public static MovementState _stateSlime;

    private bool stan;
    private bool damageNow;
    private bool isJump;

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
                number = rnd.Next(0, 100);
                isJump = false;
            }
            else timer -= Time.deltaTime;
            if (number <= chanse[0])
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position += speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.walk);
            }
            else if (number > chanse[0] && number <= chanse[1])
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                if (timer > 0) transform.position -= speed * 2 * Time.deltaTime * movingVector;
                _animator.SetInteger("state", (int)MovementState.walk);
            }
            else if (number > chanse[1] && number <= chanse[2] && !isJump)
            {
                transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isJump = true;
                _animator.SetInteger("state", (int)MovementState.jump);
                _rigidbody2D.AddForce(jumpVector, ForceMode2D.Impulse);
            }
            else if (number > chanse[2] && number <= chanse[3] && !isJump)
            {
                transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
                isJump = true;
                _animator.SetInteger("state", (int)MovementState.jump);
                _rigidbody2D.AddForce(new Vector2(-jumpVector.x,jumpVector.y), ForceMode2D.Impulse);
            }
        }
        else
        {
            fill.enabled = false;
            fillBox.enabled = false;
        }
    }

    public void Attack()
    {   
        var slimeAttack = Instantiate(slimeAttackPrefab, transform.position-new Vector3(0,0.55f,0), Quaternion.identity);
        slimeAttack.GetComponent<SlimeAttack>().catSprite = catSprite;
    }
    public void TakeDamage(float damage, bool isStan)
    {
        stan = isStan;
        if (!stan && damage > 0)
        {
            _animator.SetInteger("state", 3);
            if (!_audioSource.isPlaying && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
                _audioSource.PlayOneShot(damageClip);
            HP -= damage;
            _healthBar.SetHealth(HP);
            damageNow = true;
            if (HP <= 0 && !_animator.GetCurrentAnimatorStateInfo(0).IsName("death"))
            {
                XP.Die();
                _animator.SetInteger("state", 4);
            }
        }
    }
}
