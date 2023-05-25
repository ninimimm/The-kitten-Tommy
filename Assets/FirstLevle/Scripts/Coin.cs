using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (CoinData.start)
        {
            Save();
            CoinData.start = false;
            _spriteRenderer.enabled = true;
        }
        Load();
        audioSource = _cat.GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
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
    // Update is called once per frame
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
