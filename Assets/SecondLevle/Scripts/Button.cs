using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private Transform _pressedTransform;
    [SerializeField] private float distancePressed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private string key;
    [SerializeField] private GameObject manager;
    public enum MovementState { Stay, Pressed};
    public MovementState state;
    private Animator _animator;
    private ManageButtons _manageButtons;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _manageButtons = manager.GetComponent<ManageButtons>();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == MovementState.Stay && _manageButtons.timer < 0 && 
            Physics2D.OverlapCircle(_pressedTransform.position, distancePressed, catLayer))
        {
            state = MovementState.Pressed;
            _manageButtons.keys.Append(key);
        }
        _animator.SetInteger("state", (int)state);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_pressedTransform.position,distancePressed);
    }
}
