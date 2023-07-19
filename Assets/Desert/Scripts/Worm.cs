using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Worm : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private GameObject cat;
    [SerializeField] private Image gui;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI skip;
    private Animator _animator;
    private bool _dialog;
    
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("state", 2);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            _dialog = true;
        if (Vector3.Distance(transform.position, cat.transform.position) < 1.5)
        {
            iconText.enabled = true;
            icon.enabled = true;
        }
        else
        {
            _animator.SetInteger("state", 2);
            _dialog = false;
            iconText.enabled = false;
            icon.enabled = false;
        }
        if (Vector3.Distance(transform.position, cat.transform.position) < 1.5 && _dialog)
        {
            _animator.SetInteger("state", 1);
            gui.enabled = true;
            text1.enabled = true;
            text2.enabled = true;
            skip.enabled = true;
            iconText.enabled = false;
            icon.enabled = false;
        }
        else
        {
            gui.enabled = false;
            text1.enabled = false;
            text2.enabled = false;
            skip.enabled = false;
        }
    }
}
