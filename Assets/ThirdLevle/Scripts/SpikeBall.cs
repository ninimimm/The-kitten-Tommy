using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour
{
    [SerializeField] private CatSprite cat;
    [SerializeField] private float catDamage;
    [SerializeField] private float bossDamage;
    [SerializeField] private Boss boss;
    public PolygonCollider2D polygonCollider2D;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;
    private Vector3 _startPosition;
    private float _angle;
    void Start()
    {
        _startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        polygonCollider2D.enabled = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Cloud")
        {
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = _startPosition;
            spriteRenderer.enabled = false;
            polygonCollider2D.enabled = false;
        }
        else if (collision.gameObject.tag == "Player")
        {
            if (spriteRenderer.sprite.name == "PinkSpikesBall") boss.TakeDamage(bossDamage, false);
            else cat.TakeDamage(catDamage, false);
            rb.bodyType = RigidbodyType2D.Static;
            transform.position = _startPosition;
            spriteRenderer.enabled = false;
            polygonCollider2D.enabled = false;
        }
    }
}
