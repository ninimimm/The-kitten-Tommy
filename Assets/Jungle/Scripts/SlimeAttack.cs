using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttack : MonoBehaviour
{
    [SerializeField] private float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float damage;
    public CatSprite catSprite;
    public void Destroy()
    {
        Destroy(gameObject);
    }
    public void Hurt()
    {
        if (Physics2D.OverlapCircle(transform.position, distanseAttack, catLayer))
            catSprite.TakeDamage(damage, false);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position,distanseAttack);
    }
}
