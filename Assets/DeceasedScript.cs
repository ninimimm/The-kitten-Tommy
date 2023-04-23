using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cat;

public class DeceasedScript : MonoBehaviour
{
    [SerializeField] private float HP = 3;
    [SerializeField] private GameObject target;
    private Animator _animator;
    public enum MovementState { Deceased_Idle, Deceased_Run, Deceased_Attack, Deceased_Dead, Deceased_Hurt };
    public static MovementState _stateDeceased;
    private SpriteRenderer _deceased;
    private PolygonCollider2D _col;
    private Animator targetAnimation;
    const float speedMove = 30.0f;
    private Rigidbody2D _rb;
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _deceased = GetComponent<SpriteRenderer>();
        _col = GetComponent<PolygonCollider2D>();
        targetAnimation = target.GetComponent<Animator>();
    }

    void Update()
    {
        float direction = target.transform.position.x - transform.position.x;
        if (Mathf.Abs(direction) < 3)
        {
            Vector3 pos = transform.position;
            pos.x += Mathf.Sign(direction) * speedMove * Time.deltaTime;
            transform.position = pos;
        }
        if (transform.position.x < 35.02)
            transform.position = new Vector3(35.1f, transform.position.y, transform.position.z);
        if (HP <= 0)
        {
            _stateDeceased = MovementState.Deceased_Dead;
            transform.position = new Vector3(5, -0.6f, 0);
        }
        if (_rb.velocity.x > 0)
        {
            _stateDeceased = MovementState.Deceased_Run;
            _deceased.flipX = true;
        }
        else if (_rb.velocity.x < 0)
        {
            _stateDeceased = MovementState.Deceased_Run;
            _deceased.flipX = false;
        }
        else
            _stateDeceased = MovementState.Deceased_Idle;
        if (Vector2.Distance(transform.localPosition, target.transform.localPosition) < 1)
        {
            if (transform.position.x > target.transform.position.x) _deceased.flipX = false;
            else _deceased.flipX = true;
            _stateDeceased = MovementState.Deceased_Attack;
        }
        if (Vector3.Distance(transform.localPosition, target.transform.localPosition) < 1 && CatSprite._stateCat == CatSprite.MovementState.hit)
        {
            _stateDeceased = MovementState.Deceased_Hurt;
            HP -= 1;
        }
        _animator.SetInteger("state", (int)_stateDeceased);
    }
        
    
}