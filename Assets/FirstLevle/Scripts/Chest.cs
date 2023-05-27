using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour, IDamageable
{
    private const int MONEY_REWARD = 5;
    
    [SerializeField] private GameObject _cat;
    [SerializeField] float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private AudioSource _audioOpenSource;
    [SerializeField] private AudioSource _audioMoneySource;

    private enum MovementState { close, opened, empty};
    private MovementState _stateChest;
    private Animator _animator;
    private PolygonCollider2D[] _poly;
    private bool isOpened = false;
    private bool haveMoney = true;
    private CatSprite _catSprite;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _poly = GetComponents<PolygonCollider2D>();
        _poly[1].enabled = true;
        _poly[0].enabled = false;
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    void Update()
    {
        bool isEmpty = _animator.GetCurrentAnimatorStateInfo(0).IsName("empty");
        if (!isEmpty)
        {
            var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
            bool isOpenedAnim = _animator.GetCurrentAnimatorStateInfo(0).IsName("opened");
            if (!(catTouch.Length > 0 && isOpenedAnim))
            {
                if (isOpened)
                {
                    _audioOpenSource.Play();
                    _stateChest = MovementState.opened;
                    _audioOpenSource.Play();
                    isOpened = false;
                    _poly[0].enabled = true;
                    _poly[1].enabled = false;
                }

                _animator.SetInteger("state", (int)_stateChest);
            }
            else if (haveMoney)
            {
                _audioMoneySource.Play();
                haveMoney = false;
                _stateChest = MovementState.empty;
                _animator.SetInteger("state", (int)_stateChest);
                _catSprite.money += MONEY_REWARD;
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
