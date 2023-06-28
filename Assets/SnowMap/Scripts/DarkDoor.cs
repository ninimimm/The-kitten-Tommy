using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkDoor : MonoBehaviour
{
    [SerializeField] public AudioSource DarkDoorSource;
    private bool isSoundPlayed;
    void Update()
    {
        var animator = GetComponent<Animator>();
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Opened") && !isSoundPlayed)
        {
            DarkDoorSource.Play();
            isSoundPlayed = true;
        }
    }
}
