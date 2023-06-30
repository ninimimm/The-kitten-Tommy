    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class logicKnife : MonoBehaviour
{
    [SerializeField] public float speed = 10f;
    [SerializeField] public float damage = 1;
    public AudioSource flySource;
    public AudioSource hitSource;
    public PoisonKnife _poisonKnife;
    private SpriteRenderer _knifeSpriteRenderer;
    private Rigidbody2D _rb;
    private LogicKnifeData data;
    private float angle;
    private IDamageable enemy;
    private int index;

    // Start is called before the first frame update
    void Start()    
    {
        data = SavingSystem<logicKnife, LogicKnifeData>.Load($"{gameObject.name}.data");
        flySource.Play();
        _knifeSpriteRenderer = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = transform.right * speed;
        index = MainMenu.index;
        MainMenu.index++;
        if (MainMenu.isStarts[index] && SceneManager.GetActiveScene().name == "FirstLevle")
        {
            Save();
            MainMenu.isStarts[index] = false;
        }
        Load();
    }
    
    public void Save()
    {
        if (this != null) 
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
        if (hitInfo.gameObject.layer != 24)
        {
            var enemy = hitInfo.GetComponent<IDamageable>();
            if (enemy != null)
            {
                if (_knifeSpriteRenderer.sprite.name == "PoisonKnife")
                {
                    hitSource.Play();
                    _poisonKnife.target = enemy;
                    _poisonKnife._timers[_poisonKnife.number] = _poisonKnife.timeToPoison;
                    _poisonKnife.number++;
                }
                else
                {
                    hitSource.Play();
                    enemy.TakeDamage(damage);
                }
            }
            Destroy(gameObject);
        }
    }
}