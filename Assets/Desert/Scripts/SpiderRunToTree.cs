using UnityEngine;

public class SpiderRunToTree : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float goCoordinates;
    private Animator _animator;
    private bool isRunning;
    public bool inputE;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state",0);
    }

    // Update is called once per frame
    void Update()
    {
        if (inputE)
        {
            _animator.SetInteger("state",1);
            isRunning = true;
        }
        if (isRunning && transform.position.x > goCoordinates)
            transform.position -= speed * Time.deltaTime * new Vector3(1, 0, 0) ;
        if (transform.position.x <= goCoordinates)
            Destroy(gameObject);
    }
}