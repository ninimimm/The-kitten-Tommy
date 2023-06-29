using UnityEngine;

public class EatingBird : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator _animator;
    [SerializeField] private Transform _cat;
    [SerializeField] private float scaryDistance;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private int value;
    [SerializeField] private AudioClip flySound;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    private EatingBirdData _data;
    private SpriteRenderer _spriteRenderer;
    
    void Start()
    {
        _data = SavingSystem<EatingBird, EatingBirdData>.Load($"{gameObject.name}.data");
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", value);
        if (!_data.start.Contains(gameObject.name) && !MainMenu.isResume)
        {
            _data.start.Add(gameObject.name);
            Save();
            _spriteRenderer.enabled = true;
        }
        Load();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_spriteRenderer.enabled)
        {
            if (Vector3.Distance(_cat.position, transform.position) < scaryDistance && !_audioSource.isPlaying)
            {
                _animator.SetInteger("state", 2);
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(flySound);
            }
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += speed * Time.deltaTime * movingVector ;
            if (transform.position.y > 6) _spriteRenderer.enabled = false;
        }
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<EatingBird,EatingBirdData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        _data = SavingSystem<EatingBird,EatingBirdData>.Load($"{gameObject.name}.data");
        _animator.SetInteger("state", _data.animatorState);
        transform.position = new Vector3(
            _data.positions[gameObject.name][0],
            _data.positions[gameObject.name][1],
            _data.positions[gameObject.name][2]);
    }
}
