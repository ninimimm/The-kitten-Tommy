using UnityEngine;

public class Fly : MonoBehaviour
{
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 flyVector;
    [SerializeField] private float speedFly1;
    [SerializeField] private float speedFly2;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private bool goRight;
    private bool isFly;
    private SpriteRenderer _spriteRenderer;

    private Vector3 moveVector1;
    private Vector3 moveVector2;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        moveVector1 = new Vector3(1, 0, 0);
        moveVector2 = flyVector * speedFly2;
    }

    void Update()
    {
        if (!isFly)
        {
            if (transform.position.x < leftBound)
            {
                goRight = true;
                _spriteRenderer.flipX = true;
            }
            else if (transform.position.x > rightBound)
            {
                goRight = false;
                _spriteRenderer.flipX = false;
            }

            if (goRight) transform.position += moveVector1 * speedFly1 * Time.deltaTime;
            else transform.position -= moveVector1 * speedFly1 * Time.deltaTime;

            if (Vector3.Distance(_cat.position, transform.position) < scaryDistance)
            {
                isFly = true;
                _spriteRenderer.flipX = true;
            }
        }
        else
        {
            transform.position += moveVector2 * Time.deltaTime;
            if (transform.position.x > 30) Destroy(gameObject);
        }
    }
}
