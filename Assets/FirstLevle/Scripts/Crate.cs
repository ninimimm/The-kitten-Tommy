using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] public GameObject Cat;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _boxCollider;
    private bool getHit = false;
    private CrateData data;

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
        if (transform.position.y < -0.3 && getHit && _spriteRenderer.enabled)
        {
            var coinInstance = Instantiate(coinPrefab, transform.position, Quaternion.identity);
            var coinComponent = coinInstance.GetComponent<Coin>();
            if (coinComponent != null)
            {
                coinComponent._cat = Cat;
                coinComponent.distanseAttack = distanseAttack;
                coinComponent.catLayer = catLayer;
            }
            if (_audioSource != null)
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