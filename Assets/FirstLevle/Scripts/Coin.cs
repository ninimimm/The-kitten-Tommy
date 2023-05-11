using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] public GameObject _cat;
    [SerializeField] public float distanseAttack;
    [SerializeField] public LayerMask catLayer;
    private bool take = false;

    // Update is called once per frame
    void Update()
    {
        var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
        if (catTouch.Length > 0)
        {
            _cat.GetComponent<CatSprite>().money += 1;
            Destroy(gameObject);
        }
    }
}
