using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdStay : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = default;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y > 1.3)
        {
            _spriteRenderer.sortingLayerName = "GUI";
            _spriteRenderer.sortingOrder = 5;
        }
        else if (Vector3.Distance(_cat.position, transform.position) < scaryDistance)
        {
            _animator.SetInteger("state", 2);
            _spriteRenderer.flipX = false;
        }
            
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += movingVector * speed * Time.deltaTime;
        if (transform.position.y > 6) Destroy(gameObject);
    }
}
