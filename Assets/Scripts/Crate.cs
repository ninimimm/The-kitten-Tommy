using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject _cat;
    private bool getHit = false;

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 1 && getHit)
        {
            _cat.GetComponent<CatSprite>().money += 1;
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage) => getHit = true;
}
