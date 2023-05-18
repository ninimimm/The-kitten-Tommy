using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
    [SerializeField] private Transform _pressedTransform;
    [SerializeField] private float distancePressed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private string key;
    [SerializeField] private GameObject manager;
    public enum MovementState { Stay, Pressed};
    public MovementState state;
    private Animator _animator;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircleAll(_pressedTransform.position, distancePressed, catLayer).Length > 0 && state == MovementState.Stay)
        {
            state = MovementState.Pressed;
            manager.GetComponent<ManageButtons>().keys.Append(key);
        }
        _animator.SetInteger("state",(int)state);
    }
    private void OnDrawGizmosSelected()
    {
        if (_pressedTransform.position == null) return;
        Gizmos.DrawWireSphere(_pressedTransform.position,distancePressed);
    }
}
