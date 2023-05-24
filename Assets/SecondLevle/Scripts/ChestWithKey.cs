using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestWithKey : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    private Animator _animator;
    private Transform _catTransform;
    private bool isOpened;
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Math.Abs(transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && !isOpened)
        {
            _animator.SetInteger("state", 1);
            isOpened = true;
        }

        else if (Math.Abs(transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && isOpened)
        {
            _animator.SetInteger("state", 2);   
        }
    }
}
