using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Stone : MonoBehaviour
{
    [SerializeField] private CatSprite cat;
    [SerializeField] private float damage;
    public Rigidbody2D rigidbody2D;
    public Vector3 startPosition;
    void Start()
    {
        startPosition = transform.position;
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cloud")
        {
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            transform.position = startPosition;
        }
        else if (collision.gameObject.tag == "Player")
        {
            cat.TakeDamage(damage);
            rigidbody2D.bodyType = RigidbodyType2D.Static;
            transform.position = startPosition;
        }
    }
}
