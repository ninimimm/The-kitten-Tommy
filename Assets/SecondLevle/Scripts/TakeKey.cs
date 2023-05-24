using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeKey : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private Image goldKeyImage;
    [SerializeField] private Image ironKeyImage;
    [SerializeField] private GameObject goldKeyPrefab;
    [SerializeField] private GameObject ironKeyPrefab;
    [SerializeField] private GameObject goldChest;
    [SerializeField] private GameObject ironChest;
    
    private Animator _animatorGoldChest;
    private Animator _animatorIronChest;
    private GameObject _goldKey;
    private GameObject _ironKey;
    
    private CatSprite _catSprite;
    
    private Transform _catTransform;
    private bool isOpenedGoldChest;
    private bool isOpenedIronChest;
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _animatorGoldChest = goldChest.GetComponent<Animator>();
        _animatorIronChest = ironChest.GetComponent<Animator>();
        _catSprite = cat.GetComponent<CatSprite>();
        goldKeyImage.enabled = false;
        ironKeyImage.enabled = false;
    }

    void Update()
    {
        if (!_animatorGoldChest.GetCurrentAnimatorStateInfo(0).IsName("emptygold") ||
            !_animatorIronChest.GetCurrentAnimatorStateInfo(0).IsName("empty"))
        {
            if (!_animatorGoldChest.GetCurrentAnimatorStateInfo(0).IsName("emptygold"))
            {
                if (Math.Abs(goldChest.transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && !isOpenedGoldChest)
                {
                    _animatorGoldChest.SetInteger("state", 1);
                    isOpenedGoldChest = true;
                }
                else if (Math.Abs(goldChest.transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && isOpenedGoldChest)
                {
                    _animatorGoldChest.SetInteger("state", 2);
                    _catSprite.key = "gold";
                    goldKeyImage.enabled = true;
                    if (ironKeyImage.enabled)
                    {
                        _ironKey = Instantiate(ironKeyPrefab, 
                            new Vector3(_catSprite.transform.position.x,_catSprite.transform.position.y + 1,_catSprite.transform.position.z),
                            Quaternion.identity);
                        ironKeyImage.enabled = false;
                    }
                }
            }
            
            if (!_animatorIronChest.GetCurrentAnimatorStateInfo(0).IsName("empty"))
            {
                if (Math.Abs(ironChest.transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && !isOpenedIronChest)
                {
                    _animatorIronChest.SetInteger("state", 1);
                    isOpenedIronChest = true;
                }
                else if (Math.Abs(ironChest.transform.position.x - _catTransform.position.x) < 0.5 && Input.GetKeyDown(KeyCode.E) && isOpenedIronChest)
                {
                    _animatorIronChest.SetInteger("state", 2);
                    _catSprite.key = "iron";
                    ironKeyImage.enabled = true;
                    if (goldKeyImage.enabled)
                    {
                        _goldKey = Instantiate(goldKeyPrefab,
                            new Vector3(_catSprite.transform.position.x, _catSprite.transform.position.y + 1,
                                _catSprite.transform.position.z),
                            Quaternion.identity);
                        goldKeyImage.enabled = false;
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            if (_goldKey != null && Vector3.Distance(_catSprite.transform.position, _goldKey.transform.position) < 2)
            {
                Destroy(_goldKey);
                if (_catSprite.key == "iron")
                {
                    ironKeyImage.enabled = false;
                    _ironKey = Instantiate(ironKeyPrefab, 
                        new Vector3(_catSprite.transform.position.x,_catSprite.transform.position.y + 1,_catSprite.transform.position.z),
                        Quaternion.identity);
                }
                    
                goldKeyImage.enabled = true;
                _catSprite.key = "gold";
            }
            else if (_ironKey != null && Vector3.Distance(_catSprite.transform.position, _ironKey.transform.position) < 2)
            {
                Destroy(_ironKey);
                if (_catSprite.key == "gold")
                {
                    goldKeyImage.enabled = false;
                    _goldKey = Instantiate(goldKeyPrefab, 
                        new Vector3(_catSprite.transform.position.x,_catSprite.transform.position.y + 1,_catSprite.transform.position.z),
                        Quaternion.identity);
                }
                ironKeyImage.enabled = true;
                _catSprite.key = "iron";
            }
        }
    }
}
