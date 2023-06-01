using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform cat;
    private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(cat.transform.position, transform.position) < 1)
        {
            cat.GetComponent<SpriteRenderer>().enabled = false;
            _animator.SetInteger("state",1);
        }
            
    }

    public void LoadBoss() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
}
