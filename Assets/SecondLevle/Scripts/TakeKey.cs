using System;
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
    [SerializeField] private AudioSource openSource;
    [SerializeField] private AudioSource takeKeySource;
    
    public Animator _animatorGoldChest;
    public Animator _animatorIronChest;
    public GameObject _goldKey;
    public GameObject _ironKey;
    
    private CatSprite _catSprite;
    
    private Transform _catTransform;
    public bool isOpenedGoldChest;
    public bool isOpenedIronChest;
    private TakeKeyData data;
    private AnimatorStateInfo goldCheStateInfo;
    private AnimatorStateInfo ironCheStateInfo;
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
    }

    void Update()
    {
        goldCheStateInfo = _animatorGoldChest.GetCurrentAnimatorStateInfo(0);
        ironCheStateInfo = _animatorIronChest.GetCurrentAnimatorStateInfo(0);
        if (!goldCheStateInfo.IsName("emptygold") ||
            !ironCheStateInfo.IsName("empty"))
        {
            if (!goldCheStateInfo.IsName("emptygold"))
            {
                if (Input.GetKeyDown(KeyCode.E) && !isOpenedGoldChest &&
                    Math.Abs(goldChest.transform.position.x - _catTransform.position.x) < 0.5)
                {
                    openSource.Play();
                    _animatorGoldChest.SetInteger("state", 1);
                    isOpenedGoldChest = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && isOpenedGoldChest &&
                         Math.Abs(goldChest.transform.position.x - _catTransform.position.x) < 0.5)
                {
                    takeKeySource.Play();
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
            
            if (!ironCheStateInfo.IsName("empty"))
            {
                if (Input.GetKeyDown(KeyCode.E) && !isOpenedIronChest &&
                    Math.Abs(ironChest.transform.position.x - _catTransform.position.x) < 0.5)
                {
                    openSource.Play();
                    _animatorIronChest.SetInteger("state", 1);
                    isOpenedIronChest = true;
                }
                else if (Input.GetKeyDown(KeyCode.E) && isOpenedIronChest &&
                         Math.Abs(ironChest.transform.position.x - _catTransform.position.x) < 0.5)
                {
                    takeKeySource.Play();
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
                takeKeySource.Play();
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
            else if (_ironKey is not null && Vector3.Distance(_catSprite.transform.position, _ironKey.transform.position) < 2)
            {
                takeKeySource.Play();
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
