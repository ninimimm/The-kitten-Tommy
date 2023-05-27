using System;
using System.Collections;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField] private int fireDamage = 5;
    [SerializeField] private Transform catTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float distanseRunSourse;
    [SerializeField] private float damageInterval = 0.01f;
    private Collider2D fireCollider;
    private CatSprite playerInFire = null;
    private Coroutine damageCoroutine = null;

    private void Update()
    {
        audioSource.volume = Math.Abs(catTransform.position.x-transform.position.x) < distanseRunSourse ?
            (distanseRunSourse - Math.Abs(catTransform.position.x-transform.position.x)) / distanseRunSourse + 0.5f : 0;
        if (!audioSource.isPlaying) 
            audioSource.Play();
    }

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
