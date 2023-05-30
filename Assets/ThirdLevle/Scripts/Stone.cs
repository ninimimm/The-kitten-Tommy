using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stone : MonoBehaviour
{
    [SerializeField] private CatSprite cat;
    [SerializeField] private float damage;
    private Rigidbody2D _rigidbody2D;
    private Vector3 _startPosition;
    void Start()
    {
        _startPosition = transform.position;
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cloud")
        {
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            transform.position = _startPosition;
        }
        else if (collision.gameObject.tag == "Player")
        {
            cat.TakeDamage(damage);
            _rigidbody2D.bodyType = RigidbodyType2D.Static;
            transform.position = _startPosition;
        }
    }
}
