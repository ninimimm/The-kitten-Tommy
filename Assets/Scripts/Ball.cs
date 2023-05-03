using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Transform Radius;
    [SerializeField] private float RadiusAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float damage;
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
        if (_rb.velocity.y == 0 && transform.position.y < 3 && CanDamage)
        {
            CanDamage = false;
            foreach (var cat in hitCat)
                cat.GetComponent<CatSprite>().TakeDamage(damage);
            ballState = MovementState.fire;
            _animator.SetInteger("state",(int)ballState);
            Destroy(gameObject,1f);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (Radius.position == null)
            return;
        Gizmos.DrawWireSphere(Radius.position,RadiusAttack);
    }
}