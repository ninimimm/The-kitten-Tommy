    using UnityEngine;

public class logicKnife : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 1;
    public PoisonKnife _poisonKnife;
    private SpriteRenderer _knifeSpriteRenderer;
    private Rigidbody2D _rb;
    private LogicKnifeData data;
    private float angle;
    private IDamageable enemy;

    // Start is called before the first frame update
    void Start()
    {
        _knifeSpriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * speed;
        if (!LogicKnifeData.start.Contains(gameObject.name))
        {
            LogicKnifeData.start.Add(gameObject.name);
            Save();
        }
        Load();
    }
    
    public void Save()
    {
        SavingSystem<logicKnife,LogicKnifeData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<logicKnife, LogicKnifeData>.Load($"{gameObject.name}.data");
        damage = data.damage;
    }

    // FixedUpdate is called at fixed intervals
    void FixedUpdate()
    {
        // Calculate the rotation angle based on the velocity
        angle = Mathf.Atan2(_rb.velocity.y, _rb.velocity.x) * Mathf.Rad2Deg;

        // Set the knife's rotation
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle+30));
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        var enemy = hitInfo.GetComponent<IDamageable>();
        if (enemy != null)
        {
            if (_knifeSpriteRenderer.sprite.name == "PoisonKnife")
            {
                _poisonKnife.target = enemy;
                _poisonKnife._timer = _poisonKnife.timeToPoison;
            }
            else enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}