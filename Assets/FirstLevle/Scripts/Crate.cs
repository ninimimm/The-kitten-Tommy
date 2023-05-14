using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _cat;
    [SerializeField] private GameObject coin;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float distanseAttack;
    [SerializeField] private AudioSource _audioSource;
    private bool getHit = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -0.3 && getHit)
        {
            var _coin = Instantiate(coin, transform.position, Quaternion.identity);
            _coin.GetComponent<Coin>()._cat = _cat;
            _coin.GetComponent<Coin>().distanseAttack = distanseAttack;
            _coin.GetComponent<Coin>().catLayer = catLayer;
            _audioSource.Play();
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage) => getHit = true;
}
