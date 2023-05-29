using UnityEngine;
using UnityEngine.SceneManagement;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] public GameObject Cat;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private bool getHit;
    private CrateData data;
    private bool isStart = true;
    private GameObject coinInstance;
    private bool newSpawn = true;
    private Coin coinScript;
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!CrateData.start.Contains(gameObject.name))
        {
            CrateData.start.Add(gameObject.name);
            Save();
            _spriteRenderer.enabled = true;
        }
        Load();
        _audioSource = Cat.GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (coinScript is null && coinInstance is not null)
            coinScript = coinInstance.GetComponent<Coin>();
        if (coinInstance is not null && newSpawn)
        {
            coinInstance.transform.position = transform.position;
            newSpawn = false;
        }
            
        if (isStart)
        {
            if (SceneManager.GetActiveScene().name == "FirstLevle")
                GoToSecondLevle.crates.Add(this);
            else GoToFirstLevel.crates.Add(this);
            isStart = false;
        }
        if (transform.position.y < -0.3 && getHit && _spriteRenderer.enabled)
        {
            coinInstance = Instantiate(coinPrefab, gameObject.transform.position, Quaternion.identity);
            coinScript = coinInstance.GetComponent<Coin>();
            if (SceneManager.GetActiveScene().name == "FirstLevle")
            {
                coinInstance.name += GoToSecondLevle.countCoins.ToString();
                GoToSecondLevle.countCoins++;
            }
            else
            {
                coinInstance.name += GoToFirstLevel.countCoins.ToString();
                GoToSecondLevle.countCoins++;
            }
            
            if (coinScript is not null)
            {
                coinScript._cat = Cat;
                coinScript.distanseAttack = distanseAttack;
                coinScript.catLayer = catLayer;
            }
            if (_audioSource is not null)
            {
                _audioSource.Play();
            }
            _spriteRenderer.enabled = false;
            _boxCollider.enabled = false;
        }
    }
    public void TakeDamage(float damage) => getHit = true;

    public void Save()
    {
        SavingSystem<Crate,CrateData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Crate, CrateData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.positions[gameObject.name][0],
            data.positions[gameObject.name][1],
            data.positions[gameObject.name][2]);
    }
}