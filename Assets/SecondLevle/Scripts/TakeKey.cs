using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeKey : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] public Image goldKeyImage;
    [SerializeField] public Image ironKeyImage;
    [SerializeField] private GameObject goldKeyPrefab;
    [SerializeField] private GameObject ironKeyPrefab;
    [SerializeField] public GameObject goldChest;
    [SerializeField] public GameObject ironChest;
    
    public Animator _animatorGoldChest;
    public Animator _animatorIronChest;
    public GameObject _goldKey;
    public GameObject _ironKey;
    
    private CatSprite _catSprite;
    
    private Transform _catTransform;
    public bool isOpenedGoldChest;
    public bool isOpenedIronChest;
    private TakeKeyData data;
    private bool isStart = true;
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _animatorGoldChest = goldChest.GetComponent<Animator>();
        _animatorIronChest = ironChest.GetComponent<Animator>();
        _catSprite = cat.GetComponent<CatSprite>();
        goldKeyImage.enabled = false;
        ironKeyImage.enabled = false;
        if (!TakeKeyData.start.Contains(gameObject.name))
        {
            Save();
            TakeKeyData.start.Add(gameObject.name);
        }
        Load();
    }
    
    public void Save()
    {
        SavingSystem<TakeKey,TakeKeyData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<TakeKey, TakeKeyData>.Load($"{gameObject.name}.data");
        goldKeyImage.enabled = data.goldKeyImageEnabled;
        ironKeyImage.enabled = data.ironKeyImageEnabled;
        _animatorGoldChest.SetInteger("state",data.animatorGoldChestState);
        _animatorIronChest.SetInteger("state",data.animatorIronChestState);
        if (_goldKey != null)
        {
            _goldKey.transform.position = new Vector3(
                data.positionGoldKey[0],
                data.positionGoldKey[1],
                data.positionGoldKey[2]);
            data.goldIsNull = false;
        }
        else data.goldIsNull = true;

        if (_ironKey != null)
        {
            _ironKey.transform.position = new Vector3(
                data.positionIronKey[0],
                data.positionIronKey[1],
                data.positionIronKey[2]);
            data.ironIsNull = false;
        }
        else data.ironIsNull = true;
        isOpenedGoldChest = data.isOpenedGoldChest;
        isOpenedIronChest = data.isOpenedIronChest;
        Debug.Log("Load"+data.animatorIronChestState);
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
