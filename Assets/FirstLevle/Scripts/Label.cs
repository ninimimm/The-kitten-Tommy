using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] private Transform _cat;
    [SerializeField] private AudioSource _audioSource;

    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Broken", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (_cat.position.x > 9.1 && Input.GetKeyDown(KeyCode.Mouse1))
        {
            _audioSource.Play();
            _animator.SetBool("Broken", false);
        }
    }
}
