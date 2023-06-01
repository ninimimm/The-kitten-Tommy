using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private AudioSource PressSource;
    [SerializeField] private AudioSource UnpressSource;
    [SerializeField] private Transform _pressedTransform;
    [SerializeField] private float distancePressed;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private string key;
    [SerializeField] private GameObject manager;
    public enum MovementState { Stay, Pressed};
    public MovementState state;
    private Animator _animator;
    private ManageButtons _manageButtons;
    public bool isUnpressSoundPlayed = true;
    public bool isKeyWasPressed;
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
            isKeyWasPressed = true;
            PressSource.Play();
            _manageButtons.keys.Append(key);
        }
        _animator.SetInteger("state", (int)state);
        if (!Physics2D.OverlapCircle(_pressedTransform.position, distancePressed, catLayer) 
            && _animator.GetCurrentAnimatorStateInfo(0).IsName("stay") 
            && !isUnpressSoundPlayed && !UnpressSource.isPlaying && isKeyWasPressed)
        {
            isUnpressSoundPlayed = true;
            UnpressSource.Play();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(_pressedTransform.position,distancePressed);
    }
}
