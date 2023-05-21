using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int fireDamage = 5;
    [SerializeField] private float damageInterval = 0.01f;
    private Collider2D fireCollider;
    private CatSprite playerInFire = null;
    private Coroutine damageCoroutine = null;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cat")) 
        {
            playerInFire = other.GetComponent<CatSprite>();
            if (playerInFire != null)
            {
                damageCoroutine = StartCoroutine(DealDamage());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            playerInFire = null;
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator DealDamage()
    {
        while (playerInFire != null)
        {
            yield return new WaitForSeconds(damageInterval);
            playerInFire.TakeDamage(fireDamage);
        }
    }
}
