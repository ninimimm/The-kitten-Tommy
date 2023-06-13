using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] public GameObject Cat;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject energyPrefab;
    [SerializeField] private GameObject fishPrefab;
    [SerializeField] private GameObject waterPrefab;
    [SerializeField] private int[] chanse;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip destroyClip;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private AudioSource groundFallSource;
    [SerializeField] private AudioSource waterFallSource;
    [SerializeField] private Boosts boosts;
    [SerializeField] private int countSpawnItems;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private bool getHit;
    private CrateData data;
    private bool isStart = true;
    private GameObject coinInstance;
    private bool newSpawn = true;
    private Coin coinScript;
    private bool canFall = true;
    private GameObject energyInstance;
    private GameObject fishInstance;
    private GameObject waterInstance;
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
        _audioSource.volume = 0.2f;
        if (transform.position.y < 0)
        {
            groundFallSource.volume = 0;
            waterFallSource.volume = 0;
        }
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        CheckDistanse();
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

        if (transform.position.y < 0 && _spriteRenderer.enabled && canFall)
        {
            if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer))
            {
                canFall = false;
                groundFallSource.Play();
            }
                
            else if (Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, waterLayer))
            {
                canFall = false;
                waterFallSource.Play();
            }
        }   
        if (transform.position.y > 1) canFall = true;
        if (transform.position.y < -0.3 && getHit && _spriteRenderer.enabled)
        {
            if (_audioSource is not null)
            {
                _audioSource.PlayOneShot(destroyClip);
            }
            for (int i = 0; i < countSpawnItems; i++)
            {
                Random rnd = new Random();
                var number = rnd.Next(1, 100);
                if (number < chanse[0])
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
                }
                else if (chanse[0] < number && number < chanse[0] + chanse[1])
                    energyInstance = Instantiate(energyPrefab, gameObject.transform.position, Quaternion.identity);
                else if (chanse[0] + chanse[1] < number && number < chanse[0] + chanse[1] + chanse[2])
                    fishInstance = Instantiate(fishPrefab, gameObject.transform.position, Quaternion.identity);
                else if (chanse[0] + chanse[1] + chanse[2] < number &&
                         number < chanse[0] + chanse[1] + chanse[2] + chanse[3])
                    waterInstance = Instantiate(waterPrefab, gameObject.transform.position, Quaternion.identity); ;

                if (coinScript is not null)
                {
                    coinScript._cat = Cat;
                    coinScript.distanseAttack = distanseAttack;
                    coinScript.catLayer = catLayer;
                }
                
                _spriteRenderer.enabled = false;
                _boxCollider.enabled = false;
            }
        }
    }
    public void TakeDamage(float damage) => getHit = true;

    public void Save()
    {
        if (this != null) 
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

    private void CheckDistanse()
    {
        if (energyInstance != null && Vector3.Distance(energyInstance.transform.position, Cat.transform.position) < 1)
        {
            boosts.energyCount++;
            boosts.boostsText[0].text = "x" + boosts.energyCount;
            Destroy(energyInstance);
        }
        if (fishInstance != null && Vector3.Distance(fishInstance.transform.position, Cat.transform.position) < 1)
        {
            boosts.fishCount++;
            boosts.boostsText[1].text = "x" + boosts.fishCount;
            Destroy(fishInstance);
        }
        if (waterInstance != null && Vector3.Distance(waterInstance.transform.position, Cat.transform.position) < 1)
        {
            boosts.waterCount++;
            boosts.boostsText[2].text = "x" + boosts.waterCount;
            Destroy(waterInstance);
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(groundCheck.position,groundCheckRadius);
    }
}