using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int fireDamage = 5;
    [SerializeField] private float damageInterval = 0.01f;
    private Collider2D fireCollider;

    private void Start()
    {
        fireCollider = GetComponent<Collider2D>();
        StartCoroutine(DealDamage());
    }

    private IEnumerator DealDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(damageInterval);

            Collider2D[] collidersInsideFire = Physics2D.OverlapBoxAll(fireCollider.bounds.center, fireCollider.bounds.size, 0f);

            foreach (Collider2D collider in collidersInsideFire)
            {
                if (collider.CompareTag("Player"))
                {
                    CatSprite kitten = collider.GetComponent<CatSprite>();
                    if (kitten != null)
                    {
                        kitten.TakeDamage(fireDamage);
                    }
                }
            }
        }
    }
}