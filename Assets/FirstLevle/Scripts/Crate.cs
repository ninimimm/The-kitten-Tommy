using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] public GameObject Cat;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private AudioSource _audioSource;
    private bool getHit = false;

    void Update()
    {
        if (transform.position.y < -0.3 && getHit)
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
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage) => getHit = true;
}