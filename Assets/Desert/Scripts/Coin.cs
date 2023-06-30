using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Coin : MonoBehaviour
{
    [SerializeField] public GameObject _cat;
    [SerializeField] public float distanseAttack;
    [SerializeField] public LayerMask catLayer;
    private AudioSource audioSource;
    public AudioClip audioCoin;
    private CoinData data;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private bool isStart = true;
    private CatSprite _catSprite;
    private int index;

    private void Start()
    {
        data = SavingSystem<Coin, CoinData>.Load($"{gameObject.name}.data");
        _catSprite = _cat.GetComponent<CatSprite>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        index = MainMenu.index;
        MainMenu.index++;
        if (MainMenu.isStarts[index])
        {
            Save();
            _spriteRenderer.enabled = true;
            MainMenu.isStarts[index] = false;
        }
        Load();
        audioSource = _cat.GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        
    }

    public void Save()
    {
        if (this != null)
            SavingSystem<Coin,CoinData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        data = SavingSystem<Coin, CoinData>.Load($"{gameObject.name}.data");
        transform.position = new Vector3(
            data.positions[gameObject.name][0],
            data.positions[gameObject.name][1],
            data.positions[gameObject.name][2]);
    }
    
    
    void Update()
    {
        if (isStart)
        {
            _spriteRenderer.enabled = true;
            if (SceneManager.GetActiveScene().name == "FirstLevle")
                GoToSecondLevle.coins.Add(this);
            else if (SceneManager.GetActiveScene().name == "SecondLevle")
            {
                GoToFirstLevel.coins.Add(this);
                GoToThirdLevel.coins.Add(this);
            }
            else
                GoToSnow.coins.Add(this);
            isStart = false;
        }
        if (_spriteRenderer.enabled && Physics2D.OverlapCircle(transform.position, distanseAttack, catLayer))
        {
            _catSprite.money += 1;
            _catSprite._textMoney.text = _catSprite.money.ToString();
            audioSource.PlayOneShot(audioCoin);
            _spriteRenderer.enabled = false;
            _boxCollider.enabled = false;
        }
    }
}
