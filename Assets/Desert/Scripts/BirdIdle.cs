using UnityEngine;

public class BirdIdle : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 flyVector;
    [SerializeField] private float speedFly;
    [SerializeField] private float speedWalk;
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    [SerializeField] private AudioClip flySound;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    private bool goRight;
    private BirdIdleData _data;
    private SpriteRenderer _spriteRenderer;
    private bool isFlying;
    void Start()
    {
        _data = SavingSystem<BirdIdle, BirdIdleData>.Load($"{gameObject.name}.data");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 1);
        if (!_data.start.Contains(gameObject.name))
        {
            _data.start.Add(gameObject.name);
            Save();
            _spriteRenderer.enabled = true;
        }
        Load();
        _audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (_spriteRenderer.enabled)
        {
            isFlying = _animator.GetCurrentAnimatorStateInfo(0).IsName("fly");
            if (!isFlying)
            {
                if (transform.position.x < leftBound)
                {
                    goRight = true;
                    _spriteRenderer.flipX = false;
                }
                else if (transform.position.x > rightBound)
                {
                    goRight = false;  
                    _spriteRenderer.flipX = true;
                } 

                var movement =speedWalk * Time.deltaTime * new Vector3(goRight ? 1 : -1, 0, 0) ;
                transform.position += movement;
            }
            if ((_cat.position - transform.position).sqrMagnitude < scaryDistance * scaryDistance && !_audioSource.isPlaying)
            {
                _animator.SetInteger("state", 2);
                _spriteRenderer.flipX = true;
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(flySound);
            }
            if (isFlying) transform.position += speedFly * Time.deltaTime * flyVector ;
            if (transform.position.y > 6)
            {
                _spriteRenderer.enabled = false;
            }
        }
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<BirdIdle,BirdIdleData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        _data = SavingSystem<BirdIdle, BirdIdleData>.Load($"{gameObject.name}.data");
        _animator.SetInteger("state", _data.animatorState);
        transform.position = new Vector3(
            _data.positions[gameObject.name][0],
            _data.positions[gameObject.name][1],
            _data.positions[gameObject.name][2]);
    }
}