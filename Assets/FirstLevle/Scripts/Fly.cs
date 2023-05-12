using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fly : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 flyVector;
    [SerializeField] private float speedFly1;
    [SerializeField] private float speedFly2;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private bool goRight;
    private bool isFly;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x < leftBound)
        {
            goRight = true;
            _spriteRenderer.flipX = true;
        }
        else if (transform.position.x > rightBound)
        {
            goRight = false;
            _spriteRenderer.flipX = false;
        } 
        if (goRight)
            transform.position += new Vector3(1,0,0) * speedFly1;
        else
            transform.position -= new Vector3(1,0,0)* speedFly1;
        if (Vector3.Distance(_cat.position, transform.position) < scaryDistance && isFly == false)
        {
            isFly = true;
            _spriteRenderer.flipX = true;
        }
        if (isFly) transform.position += flyVector * speedFly2 * Time.deltaTime;
        if (transform.position.x > 30) Destroy(gameObject);
    }
}