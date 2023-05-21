using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] private Transform _cat;
    [SerializeField] private AudioSource _audioSource;

    private Animator _animator;
    private bool isIdleLabel = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Broken", true);
    }

    private void Update()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isIdleLabelState = stateInfo.IsName("IdleLabel");

        if (_cat.position.x > 9.1f && Input.GetKeyDown(KeyCode.Mouse1) && !isIdleLabelState)
        {
            _audioSource.Play();
            _animator.SetBool("Broken", false);
            isIdleLabel = false;
        }
        else
        {
            isIdleLabel = isIdleLabelState;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat") && _cat.position.x > 9.1f && !isIdleLabel)
        {
            _audioSource.Play();
            _animator.SetBool("Broken", false);
            isIdleLabel = false;
        }
    }
}