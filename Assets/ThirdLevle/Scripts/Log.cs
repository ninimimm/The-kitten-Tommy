using UnityEngine;

public class Log : MonoBehaviour
{
    public CatSprite _cat;
    public GameObject _boss;
    public float damage;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && _boss != null && _cat != null)
        {
            _cat.TakeDamage(damage);
            _boss.GetComponent<Boss>().LogDestroyed(this);
        }
    }
}


