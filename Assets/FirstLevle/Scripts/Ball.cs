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
        var hitCat = Physics2D.OverlapCircleAll(Radius.position, RadiusAttack, catLayer);
        var player = Physics2D.OverlapCircleAll(DestroyRadius.position, RadiusDestroy, catLayer);
        var Ground = Physics2D.OverlapCircleAll(DestroyRadius.position, RadiusDestroy, ground);
        var Enemies = Physics2D.OverlapCircleAll(DestroyRadius.position, RadiusDestroy, enemies);
        if (CanDamage && player.Length > 0)
        {
            CanDamage = false;
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);
            ballState = MovementState.fire;
            _animator.SetInteger("state",(int)ballState);
            Destroy(gameObject,0.6f);
        }
        else if (Ground.Length > 0 || Enemies.Length > 0)
        {
            ballState = MovementState.fire;
            _animator.SetInteger("state",(int)ballState);
            _audioSource.Play();
            Destroy(gameObject,0.6f);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (Radius.position == null)
            return;
        Gizmos.DrawWireSphere(Radius.position,RadiusAttack);
    }
}