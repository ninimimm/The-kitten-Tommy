using UnityEngine;

public class SpiderJump : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float goCoordinates;
    private Animator _animator;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rigidbody2D;
    private bool isRunning;
    private bool isJump;
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

        if (isRunning && transform.position.x < goCoordinates)
            transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        else if (isRunning && transform.position.x >= goCoordinates && isJump == false)
        {
            _animator.SetInteger("state",2);
            _rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isJump = true;
        }
        else if (isJump)
            transform.position += new Vector3(1, 0, 0) * speed * Time.deltaTime;
        if (transform.position.y < -1.5f)
            Destroy(gameObject);
    }
}
