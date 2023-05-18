using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private Transform attack;
    [SerializeField] private float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float damage;
    [SerializeField] private float timeToWait;
    [SerializeField] private float timeToPoison;
    [SerializeField] private float timeToStay;
    private Animator _animator;
    private float timer = -0.1f;
    private float timerWait;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (timeToStay > 0)
        {
            timeToStay -= Time.deltaTime;
            _animator.enabled = false;
        }
        else
        {
            _animator.enabled = true;
            var hit = new Collider2D[0];
            if (_spriteRenderer.sprite.name == "Spiders_8")
            {
                hit = Physics2D.OverlapCircleAll(attack.position, distanseAttack, catLayer);
                if (hit.Length > 0)
                    timer = timeToPoison;
            }

            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                timerWait -= Time.deltaTime;
                if (timerWait < 0)
                {
                    cat.GetComponent<CatSprite>().TakeDamage(damage);
                    timerWait = timeToWait;
                }
            }
        }
    }
}
