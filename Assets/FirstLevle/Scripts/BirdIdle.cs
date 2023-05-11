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

    // Update is called once per frame
    void Update()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("fly"))
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
            if (goRight)
                transform.position += new Vector3(1,0,0) * speedWalk;
            else
                transform.position -= new Vector3(1,0,0)* speedWalk;
        }
        if (Vector3.Distance(_cat.position, transform.position) < scaryDistance && !_audioSource.isPlaying)
        {
            _animator.SetInteger("state", 2);
            _spriteRenderer.flipX = true;
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(flySound);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += flyVector * speedFly * Time.deltaTime;
        if (transform.position.y > 6) Destroy(gameObject);
    }
}