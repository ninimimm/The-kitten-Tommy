using Unity.VisualScripting;
using UnityEngine;

public class XpMovement : MonoBehaviour
{
    public GameObject cat;
    [SerializeField] private float speed = 1.0f;
    [SerializeField] private float increaseFactor = 0.5f;
    [SerializeField] private LayerMask catLayer;
    private CatSprite _catSprite;
    [SerializeField] public float timer = 1;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        _catSprite = cat.GetComponent<CatSprite>();
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            Vector2 direction = (cat.transform.position - transform.position-new Vector3(0,0.5f,0)).normalized;
            float distance = Vector2.Distance(cat.transform.position, transform.position);
            float currentSpeed = speed + increaseFactor / distance;
            rb.velocity = direction * currentSpeed;
            if (Physics2D.OverlapCircle(transform.position, 0.5f, catLayer))
            {
                _catSprite.XP += 1;
                _catSprite.greenBar.SetHealth(_catSprite.XP);
                Destroy(gameObject);
            }
        }
    }
}