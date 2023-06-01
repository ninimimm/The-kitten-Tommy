using System;
using System.Collections;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField] private Transform catTransform;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float distanseRun;

    private void Update()
    {
        var catVector = catTransform.position;
        var vector = transform.position;
        var dy = catVector.y - vector.y;
        var dx = catVector.x - vector.x;
        var distance = Math.Sqrt(dx * dx + dy * dy);
        audioSource.volume = 0.4f;
        audioSource.volume -= (float)(distance < distanseRun ? 0.4f - (distanseRun - distance) / (2.5 * distanseRun): 0.4f);
        if (!audioSource.isPlaying) 
            audioSource.Play();
    }
}