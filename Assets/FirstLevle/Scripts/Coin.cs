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
    private bool take = false;

    private void Start()
    {
        audioSource = _cat.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
        if (catTouch.Length > 0)
        {
            _cat.GetComponent<CatSprite>().money += 1;
            audioSource.PlayOneShot(audioCoin);
            Destroy(gameObject);
        }
    }
}
