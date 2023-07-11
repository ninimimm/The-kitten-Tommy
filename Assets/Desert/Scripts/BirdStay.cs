using UnityEngine;

public class BirdStay : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip flySound;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    private SpriteRenderer _spriteRenderer;
    private BirdStayData _data;
    void Start()
    {
        _data = SavingSystem<BirdStay, BirdStayData>.Load($"{gameObject.name}.data");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 0);
        _spriteRenderer.sortingLayerName = default;
        if (!_data.start.Contains(gameObject.name))
        {
            _data.start.Add(gameObject.name);
            Save();
            _spriteRenderer.enabled = true;
        }
        Load();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (_spriteRenderer.enabled)
        {
            if ((_cat.position - transform.position).sqrMagnitude < scaryDistance * scaryDistance && !_audioSource.isPlaying)
            {
                _animator.SetInteger("state", 2);
                _spriteRenderer.flipX = false;
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(flySound);
            }
            else if (transform.position.y > 1.3 && _spriteRenderer.sortingLayerName != "GUI")
            {
                _spriteRenderer.sortingLayerName = "GUI";
                _spriteRenderer.sortingOrder = 5;
            }
        
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += speed * Time.deltaTime * movingVector;
            if (transform.position.y > 6) _spriteRenderer.enabled = false;
        }
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<BirdStay,BirdStayData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        _data = SavingSystem<BirdStay,BirdStayData>.Load($"{gameObject.name}.data");
        _animator.SetInteger("state", _data.animatorState);
        transform.position = new Vector3(
            _data.positions[gameObject.name][0],
            _data.positions[gameObject.name][1],
            _data.positions[gameObject.name][2]);
    }
}