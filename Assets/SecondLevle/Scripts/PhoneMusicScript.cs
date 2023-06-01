using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneMusicScript : MonoBehaviour
{
    [SerializeField] private Transform catTransform;
    [SerializeField] private AudioSource CavernSource;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        var catVector = catTransform.position;
        if (catVector.y > 3.6)
        {
            CavernSource.Stop();
            if (!audioSource.isPlaying)
                audioSource.Play();
            return;
        }

        if (catVector.x > -16 && catVector.x < -14)
        {
            if (catVector.y > -2)
            {
                CavernSource.Stop();
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                if (!CavernSource.isPlaying)
                    CavernSource.Play();
            }
        }

        if (catVector.y < -4.8 && catVector.y > -7.3)
        {
            if (catVector.x > 44)
            {
                CavernSource.Stop();
                if (!audioSource.isPlaying)
                    audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                if (!CavernSource.isPlaying)
                    CavernSource.Play();
            }
        }
    }
}
