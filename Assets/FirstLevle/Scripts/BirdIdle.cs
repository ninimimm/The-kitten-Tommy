using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdIdle : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 flyVector;
    [SerializeField] private float speedFly;
    [SerializeField] private float speedWalk;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private AudioClip flySound;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    private bool goRight;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 1);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        bool isFlying = _animator.GetCurrentAnimatorStateInfo(0).IsName("fly");
        if (!isFlying)
        {
            if (transform.position.x < leftBound)
            {
                goRight = true;
                _spriteRenderer.flipX = false;
            }
            else if (transform.position.x > rightBound)
            {
                goRight = false;  
                _spriteRenderer.flipX = true;
            } 

            var movement = new Vector3(goRight ? 1 : -1, 0, 0) * speedWalk * Time.deltaTime;
            transform.position += movement;
        }
        if ((_cat.position - transform.position).sqrMagnitude < scaryDistance * scaryDistance && !_audioSource.isPlaying)
        {
            _animator.SetInteger("state", 2);
            _spriteRenderer.flipX = true;
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(flySound);
        }
        if (isFlying) transform.position += flyVector * speedFly * Time.deltaTime;
        if (transform.position.y > 6) Destroy(gameObject);
    }
}