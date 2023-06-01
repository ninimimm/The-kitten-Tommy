using System;
using UnityEngine;

public class HotSand : MonoBehaviour
{
    [SerializeField] private float slowFactor = 0.5f;
    [SerializeField] private AudioSource _audioSource;

    private CatSprite kitten;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
            if (kitten == null)
                kitten = collision.GetComponent<CatSprite>();

            if (kitten != null)
            {
                kitten.SetSpeedMultiplier(slowFactor);
                kitten.TakeDamage(0.1f);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (kitten != null)
                kitten.SetSpeedMultiplier(1f);
            _audioSource.Stop();
        }
    }
}