using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour, IDamageable
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _cat;
    [SerializeField] float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    public enum MovementState { close, opened, empty};
    private MovementState _stateChest;
    private Animator _animator;
    private PolygonCollider2D[] _poly;
    private bool isOpened = false;
    private bool haveMoney = true;
    
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _poly = GetComponents<PolygonCollider2D>();
        _poly[1].enabled = true;
        _poly[0].enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("empty"))
        {
            var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
            if (!(catTouch.Length > 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("opened")))
            {
                if (isOpened)
                {
                    _stateChest = MovementState.opened;
                    isOpened = false;
                    _poly[0].enabled = true;
                    _poly[1].enabled = false;
                }

                _animator.SetInteger("state", (int)_stateChest);
            }
            else if (haveMoney)
            {
                haveMoney = false;
                _stateChest = MovementState.empty;
                _animator.SetInteger("state", (int)_stateChest);
                _cat.GetComponent<CatSprite>().money += 5;
            }
        }
    }
    public void TakeDamage(float damage)
    {
        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("empty"))
        {
            _stateChest = MovementState.opened;
            isOpened = true;
        }
    }
}
