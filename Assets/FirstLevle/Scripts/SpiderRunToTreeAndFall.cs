using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderRunToTreeAndFall : MonoBehaviour
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
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("fall"))
        {
            if (Input.GetKey(KeyCode.Mouse1))
            {
                _animator.SetInteger("state",1);
                isRunning = true;
            }
            if (isRunning && transform.position.x > goCoordinates)
                transform.position -= new Vector3(1, 0, 0) * speed * Time.deltaTime;
            if (transform.position.x <= goCoordinates)
                _animator.SetInteger("state", 3);
        }
        else _animator.SetInteger("state",4);
        
    }
}