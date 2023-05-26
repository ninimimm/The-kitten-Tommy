using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (!CoinData.start.Contains(gameObject.name))
        {
            CoinData.start.Add(gameObject.name);
            Save();
            _spriteRenderer.enabled = true;
        }
        Load();
        audioSource = _cat.GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        if (SceneManager.GetActiveScene().name == "FirstLevle")
            GoToSecondLevle.coins.Add(gameObject);
        else GoToFirstLevel.coins.Add(gameObject);
    }

    public void Save()
    {
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
        if (_spriteRenderer.enabled)
        {
            var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
            if (catTouch.Length > 0)
            {
                _cat.GetComponent<CatSprite>().money += 1;
                audioSource.PlayOneShot(audioCoin);
                _spriteRenderer.enabled = false;
                _boxCollider.enabled = false;
            }
        }
    }
}
