using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    [SerializeField] private Boosts boosts;
    [SerializeField] private AudioSource _audioSource;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector3.Distance(_cat.transform.position, transform.position) < 1)
        {
            _cat.GetComponent<SpriteRenderer>().enabled = false;
            _animator.SetInteger("state",1);
            _audioSource.Play();
        }
        if (_animator.GetInteger("state") == 1) Save();
    }
    private void Save()
    {
        _cat.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
