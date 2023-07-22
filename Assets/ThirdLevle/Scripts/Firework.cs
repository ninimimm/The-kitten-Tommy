using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Firework : MonoBehaviour
{
    [SerializeField] private float timeToWait;
    [SerializeField] private int speed1;
    [SerializeField] private int speed2;
    [SerializeField] private int upBound1;
    [SerializeField] private int upBound2;
    [SerializeField] private AudioClip start;
    [SerializeField] private AudioClip fly;
    [SerializeField] private AudioClip fire;
    private Animator _animator;

    private float bound;
    private float speed;

    private AudioSource _audioSource;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        var rnd = new Random();
        bound = rnd.Next(upBound1, upBound2)/100;
        speed = rnd.Next(speed1, speed2);
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToWait < 0)
        {
            _audioSource.volume = 0.4f;
            if (transform.position.y < bound) transform.position += new Vector3(0, speed, 0) * Time.deltaTime;
            else
            {
                var rnd = new Random();
                var number = rnd.Next(1, 5);
                _animator.SetInteger("state", number);
            }
        }
        else
        {
            timeToWait -= Time.deltaTime;
            if (timeToWait < 0)
            {
                _audioSource.volume = 0.2f;
                _audioSource.PlayOneShot(start);
            }
        }
    }

    public void Destroy() => Destroy(gameObject);
    public void Fire() => _audioSource.PlayOneShot(fire);
}
