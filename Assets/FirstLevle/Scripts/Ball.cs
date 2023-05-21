using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform Radius;
    [SerializeField] private Transform DestroyRadius;
    [SerializeField] private float RadiusAttack;
    [SerializeField] private float RadiusDestroy;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private LayerMask ground;
    [SerializeField] private LayerMask enemies;
    [SerializeField] private float damage;
    public AudioSource _audioSource;
    private enum MovementState
    {
        fall,
        fire
    }
    private MovementState ballState = MovementState.fall;
    private Animator _animator;
    private Rigidbody2D _rb;
    private bool CanDamage = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        var hitObjects = Physics2D.OverlapCircleAll(DestroyRadius.position, RadiusDestroy, catLayer | ground | enemies);
        if (hitObjects.Length > 0)
        {
            bool playerHit = false;
            bool groundOrEnemyHit = false;

            foreach (var hitObject in hitObjects)
            {
                if ((catLayer.value & (1 << hitObject.gameObject.layer)) != 0) // hitObject is in catLayer
                {
                    if (CanDamage)
                    {
                        CanDamage = false;
                        hitObject.GetComponent<CatSprite>().TakeDamage(damage);
                        playerHit = true;
                    }
                }
                else if ((ground.value & (1 << hitObject.gameObject.layer)) != 0 || 
                         (enemies.value & (1 << hitObject.gameObject.layer)) != 0) // hitObject is in ground or enemies
                    groundOrEnemyHit = true;
            }

            if (playerHit || groundOrEnemyHit)
            {
                ballState = MovementState.fire;
                _animator.SetInteger("state",(int)ballState);
                if (!groundOrEnemyHit) // Avoid multiple calls to _audioSource.Play() when both player and ground/enemy are hit
                    _audioSource.Play();
                Destroy(gameObject, 0.6f);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (Radius.position == null)
            return;
        Gizmos.DrawWireSphere(Radius.position,RadiusAttack);
    }
}