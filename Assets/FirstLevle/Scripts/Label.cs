using UnityEngine;

public class Label : MonoBehaviour
{
    [SerializeField] private Transform _cat;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private TextInTable _textInTable;
    [SerializeField] private SpiderJump _spiderJump;
    [SerializeField] private SpiderRunToTree _spiderRunToTree;
    [SerializeField] private SpiderRunToTreeAndFall _spiderRunToTreeAndFall;

    private Animator _animator;
    private bool isIdleLabel;
    private AnimatorStateInfo stateInfo;
    private bool isIdleLabelState;


    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Broken", true);
    }

    private void Update()
    {
        stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        isIdleLabelState = stateInfo.IsName("IdleLabel");

        if (Vector3.Distance(_cat.transform.position,transform.position) < 1 && Input.GetKeyDown(KeyCode.E) && !isIdleLabelState)
        {
            _audioSource.Play();
            _animator.SetBool("Broken", false);
            isIdleLabel = false;
            _textInTable.inputE = true;
            _spiderJump.inputE = true;
            _spiderRunToTree.inputE = true;
            _spiderRunToTreeAndFall.inputE = true;
        }
        else
            isIdleLabel = isIdleLabelState;
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