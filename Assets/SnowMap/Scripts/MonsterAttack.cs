using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private LayerMask destroyLayers;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float damage;
    public CatSprite catSprite;
    private Vector3 currentTransform;

    void Start()
    {
        currentTransform = catSprite.transform.position-new Vector3(0,0.4f,0);
    }

    void Update()
    {
        Vector3 direction = currentTransform - transform.position;
        transform.position += speed * Time.deltaTime * direction.normalized;
        currentTransform += speed * Time.deltaTime * direction.normalized;

        if (Physics2D.OverlapCircle(transform.position, 0.1f, destroyLayers))
        {
            if (Physics2D.OverlapCircle(transform.position, 0.1f, catLayer))
                catSprite.TakeDamage(damage, false);
            Destroy(gameObject);
        }
            
        if (direction != Vector3.zero)
        {
            transform.LookAt(transform.position + direction);
            transform.Rotate(0, -90, 0); // Для корректного поворота спрайта, если ось "forward" направлена вдоль оси Z
        }
    }
}