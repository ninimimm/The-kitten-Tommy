using System.Collections;
using System.Collections.Generic;
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
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", value);
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_cat.position, transform.position) < scaryDistance && !_audioSource.isPlaying)
        {
            _animator.SetInteger("state", 2);
            _audioSource.volume = volume;
            _audioSource.PlayOneShot(flySound);
        }
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName("fly")) transform.position += movingVector * speed * Time.deltaTime;
        if (transform.position.y > 6) Destroy(gameObject);
    }
}
