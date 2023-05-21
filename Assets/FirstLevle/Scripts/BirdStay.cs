using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 0);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = default;
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (transform.position.y > 1.3 && _spriteRenderer.sortingLayerName != "GUI")
        {
            _spriteRenderer.sortingLayerName = "GUI";
            _spriteRenderer.sortingOrder = 5;
        }
        else if ((_cat.position - transform.position).sqrMagnitude < scaryDistance * scaryDistance && !_audioSource.isPlaying)
        {
            _animator.SetInteger("state", 2);
            _spriteRenderer.flipX = false;
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(flySound);
        }
        
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += movingVector * speed * Time.deltaTime;
        if (transform.position.y > 6) Destroy(gameObject);
    }
}
