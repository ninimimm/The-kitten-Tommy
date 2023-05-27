using UnityEngine;
using UnityEngine.UI;

public class Hyena : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _cat;
    [SerializeField] private float distanceWalk;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private Transform attack;
    [SerializeField] private float distanseAttack;
    [SerializeField] private float damage;
    [SerializeField] private float maxHP;
    [SerializeField] public float HP;
    [SerializeField] private HealthBar _healthBar;
    [SerializeField] private Image _fill;
    [SerializeField] private Image _bar;
    public PolygonCollider2D pol;
    public CapsuleCollider2D cap;
    private bool damageNow = false;
    public enum MovementState { stay, walk, attake, death, hurt };
    public MovementState stateHyena;
    private Vector3 coordinates;
    private Rigidbody2D _rb;
    private Vector3 delta;
    public Animator animator;
    private HyenaData data;
    private CatSprite catSprite;
    private bool isStart = true;

    // Start is called before the first frame update
    void Start()
    {
        HP = maxHP;
        _healthBar.SetMaxHealth(maxHP);
        coordinates = transform.position;
        _rb = GetComponent<Rigidbody2D>();
        pol = GetComponent<PolygonCollider2D>();
        cap = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        pol.enabled = false;
        cap.enabled = true;
        catSprite = _cat.GetComponent<CatSprite>();
        if (!SnakeData.start.Contains(gameObject.name))
        {
            HP = maxHP;
            _healthBar.SetMaxHealth(maxHP);
            Save();
            SnakeData.start.Add(gameObject.name);
        }
        Load();
        _healthBar.SetMaxHealth(maxHP);
        _healthBar.SetHealth(HP);
    }

    public void Save()
    {
        SavingSystem<Hyena,HyenaData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Hyena, HyenaData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.position[0],
            data.position[1],
            data.position[2]);
        HP = data.HP;
        pol.enabled = data.polyEnabled;
        cap.enabled = data.capEnabled;
        animator.SetInteger("state",data.animatorState);
    }
    
    // Update is called once per frame
    void Update()
    {
        var currentAnimatorState = animator.GetCurrentAnimatorStateInfo(0);
        if (!currentAnimatorState.IsName("HyenaDeath"))
            ProcessMovementAndAttack(currentAnimatorState);
        else
            HandleDeathState();
        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    private void ProcessMovementAndAttack(AnimatorStateInfo currentAnimatorState)
    {
        Vector2 direction = GetMovementDirection();
        UpdateStateBasedOnHealth();
        Flip(direction.x);
        _rb.velocity = direction * speed;
        animator.SetInteger("state", (int)stateHyena);
        if (!currentAnimatorState.IsName("HyenaAttack")) Attack();
    }

    private Vector2 GetMovementDirection()
    {
        Vector2 direction = new Vector2(0, 0);
        if (Vector3.Distance(_cat.transform.position, coordinates) < distanceWalk)
        {
            direction.x = _cat.transform.position.x > transform.position.x ? 1 : -1;
            stateHyena = MovementState.walk;    
        }
        else if (Vector3.Distance(_cat.transform.position, coordinates) >= distanceWalk &&
                 Vector3.Distance(coordinates, transform.position) > 0.5)
        {
            direction = (coordinates - transform.position);
            stateHyena = MovementState.walk;
        }
        else
            stateHyena = MovementState.stay;

        return direction;
    }

    private void UpdateStateBasedOnHealth()
    {
        if (damageNow && HP > 0)
        {
            stateHyena = MovementState.hurt;
            damageNow = false;
        }
        else if (damageNow && HP <= 0)
            stateHyena = MovementState.death;
    }

    private void HandleDeathState()
    {
        pol.enabled = true;
        cap.enabled = false;
        _fill.enabled = false;
        _bar.enabled = false;
    }

    void Attack()
    {
        var hitCat = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
        if (hitCat.Length > 0)
            stateHyena = MovementState.attake;
        foreach (var cat in hitCat)
            catSprite.TakeDamage(damage);
    }
    
    public void TakeDamage(float damage)
    {
        HP -= damage;
        _healthBar.SetHealth(HP);
        damageNow = true;
    }
    private void Flip(float horizontalDirection)
    {
        if (horizontalDirection > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalDirection < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attack.position == null)
            return;
        Gizmos.DrawWireSphere(attack.position,distanseAttack);
    }
}
