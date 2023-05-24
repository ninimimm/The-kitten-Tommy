using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private string key;
    [SerializeField] private GameObject logicKeys;
    private TakeKey _takeKey;
    private BoxCollider2D _boxCollider2D;
    private Animator _animator;
    private CatSprite _catSprite;
    void Start()
    {
        _takeKey = logicKeys.GetComponent<TakeKey>();
        _boxCollider2D = GetComponent<BoxCollider2D>();
        _catSprite = cat.GetComponent<CatSprite>();
        _animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Math.Abs(transform.position.x - cat.transform.position.x) < 1 && _catSprite.key == key && Input.GetKeyDown(KeyCode.E))
        {
            _animator.SetInteger("state", 1);
            _catSprite.key = "";
            _boxCollider2D.enabled = false;
            if (key == "iron") _takeKey.ironKeyImage.enabled = false;
            else _takeKey.goldKeyImage.enabled = false;
        }
    }
}
