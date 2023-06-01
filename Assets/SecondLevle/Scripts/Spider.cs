using UnityEngine;

public class Spider : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private Transform attack;
    [SerializeField] private float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private float damage;
    [SerializeField] private float timeToWait;
    [SerializeField] private float timeToPoison;
    [SerializeField] private float timeToStay;
    private Animator _animator;
    private float timer = -0.1f;
    private float timerWait;
    private SpriteRenderer _spriteRenderer;
    private CatSprite _catSprite;
    void Start()
    {
        _catSprite = cat.GetComponent<CatSprite>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (timeToStay > 0)
        {
            timeToStay -= Time.deltaTime;
            _animator.enabled = false;
        }
        else
        {
            _animator.enabled = true;
            if (_spriteRenderer.sprite.name == "Spiders_8" &&
                Physics2D.OverlapCircle(attack.position, distanseAttack, catLayer))
                timer = timeToPoison;

            if (timer >= 0)
            {
                timer -= Time.deltaTime;
                timerWait -= Time.deltaTime;
                if (timerWait < 0)
                {
                    _catSprite.TakeDamage(damage);
                    timerWait = timeToWait;
                }
            }
        }
    }
}
