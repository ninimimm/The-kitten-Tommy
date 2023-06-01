using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToThirdLevel : MonoBehaviour
{
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_animator.GetInteger("state") == 1) Save();
    }
    private void Save()
    {
        _cat.Save();
        knife.Save();
        logicKnife.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
