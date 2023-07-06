 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Experience : MonoBehaviour
{
    [SerializeField] private float timer = 1;
    [SerializeField] private Transform dieObject;
    [SerializeField] private GameObject cat;
    [SerializeField] private GameObject prefab;
    [SerializeField] private int minXP = 1;
    [SerializeField] private int maxXP = 5;
    [SerializeField] private float xpForce = 5.0f;

    public void Die()
    {
        var xpCount = Random.Range(minXP, maxXP + 1);
        for (var i = 0; i < xpCount; i++)
        {
            var xp = Instantiate(prefab, dieObject.position, Quaternion.identity);
            xp.GetComponent<XpMovement>().cat = cat;
            xp.GetComponent<XpMovement>().timer = timer;
            var rb = xp.GetComponent<Rigidbody2D>();
            var direction = new Vector2(Random.Range(-1.0f, 1.0f), 1);
            rb.AddForce(direction.normalized * xpForce, ForceMode2D.Impulse);
        } 
    }
}
