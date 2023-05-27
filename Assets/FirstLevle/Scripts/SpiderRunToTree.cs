using UnityEngine;

public class SpiderRunToTree : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float goCoordinates;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody2D;
    private bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _animator.SetInteger("state",0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            _animator.SetInteger("state",1);
            isRunning = true;
        }
        if (isRunning && transform.position.x > goCoordinates)
            transform.position -= new Vector3(1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.x <= goCoordinates)
            Destroy(gameObject);
    }
}