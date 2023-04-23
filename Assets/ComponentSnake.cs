using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Cat;
using UnityEngine;

namespace SnakeTarget
{
    public class ComponentSnake : MonoBehaviour
    {
        [SerializeField] private float HP = 3;
        [SerializeField] private GameObject target;
        private Animator _animator;
        public enum MovementState { Stay, Walk, attake, death, hurt };
        public static MovementState _stateSnake;
        private SpriteRenderer _snake;
        private PolygonCollider2D _col;
        private Animator targetAnimation;

        private Rigidbody2D _rb;
        // Start is called before the first frame update
        void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _snake = GetComponent<SpriteRenderer>();
            _col = GetComponent<PolygonCollider2D>();
            targetAnimation = target.GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {
            if (transform.position.x > 12.7)
                transform.position = new Vector3(12.69f,transform.position.y,transform.position.z);
            if (HP < 0)
            {
                _stateSnake = MovementState.death;
                transform.position = new Vector3(5, -0.6f, 0);
            }
            if (_rb.velocity.x > 0)
            {
                _stateSnake = MovementState.Walk;
                _snake.flipX = true;
            }
            else if (_rb.velocity.x < 0)
            {
                _stateSnake = MovementState.Walk;
                _snake.flipX = false;
            }
            else
                _stateSnake = MovementState.Stay;
            if (Vector2.Distance(transform.localPosition, target.transform.localPosition) < 1 && CatSprite._stateCat != CatSprite.MovementState.hit)
            {
                if (transform.position.x > target.transform.position.x) _snake.flipX = false;
                else _snake.flipX = true;
                _stateSnake = MovementState.attake;
            }
            if (Vector3.Distance(transform.localPosition, target.transform.localPosition) < 1 && CatSprite._stateCat == CatSprite.MovementState.hit)
            {
                _stateSnake = MovementState.hurt;
                HP -= 1;
            }
            _animator.SetInteger("state", (int)_stateSnake);
        }
    }
}
