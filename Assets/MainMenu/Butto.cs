using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Butto : MonoBehaviour
{
    [SerializeField] private AudioSource superSource;
    [SerializeField] private AudioSource clickSource;
    public void PlaySuper()
    {
        superSource.Play();
    }

    public void PlayClick()
    {
        clickSource.Play();
    }
}
