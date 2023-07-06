using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveNut : MonoBehaviour
{
    [SerializeField] private float takeDamage;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask destroyMask;
    public bool moveRight;
    void Update()
    {
        var hitObj = Physics2D.OverlapCircle(transform.position, 0.1f, destroyMask);
        if (hitObj)
        {
            if (Physics2D.OverlapCircle(transform.position, 0.1f, 7)) Destroy(gameObject);
            hitObj.GetComponent<CatSprite>()?.TakeDamage(takeDamage, false);
        }
        if (moveRight)
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);
    }
}
