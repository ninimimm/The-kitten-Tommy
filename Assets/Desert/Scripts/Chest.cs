using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private const int MONEY_REWARD = 5;
    [SerializeField] private GameObject _cat;
    [SerializeField] float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private AudioSource _audioOpenSource;
    [SerializeField] private AudioSource _audioMoneySource;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private TextMeshProUGUI textIcon;
    [SerializeField] public BoxCollider2D[] _poly;
    public Animator _animator;
    public bool isOpened;
    private CatSprite _catSprite;
    private ChestData _data;
    private bool isStart = true;
    private int index = -1;
    public bool isEmpty;

    void Start()
    {
        _data = SavingSystem<Chest, ChestData>.Load($"{gameObject.name}.data");
        icon.enabled = false;
        textIcon.enabled = false;
        _animator = GetComponent<Animator>();
        _poly = GetComponents<BoxCollider2D>();
        _catSprite = _cat.GetComponent<CatSprite>();
        _poly[1].enabled = true;
        _poly[0].enabled = false;
        _animator.SetInteger("state", 0);
        if (!MainMenu.dictSave.ContainsKey(gameObject.name))
        {
            MainMenu.dictSave.Add(gameObject.name,MainMenu.index);
            MainMenu.index ++;
        }
        if (MainMenu.isStarts[MainMenu.dictSave[gameObject.name]])
        {
            Save();
            MainMenu.isStarts[MainMenu.dictSave[gameObject.name]] = false;
        }
        Load();
    }

    void Update()
    {
        if (!isEmpty)
        {
            if (isOpened)
            {
                _animator.SetInteger("state", 1);
                
                if (icon.enabled && Input.GetKeyDown(KeyCode.E) )
                {
                    _audioMoneySource.Play();
                    _catSprite.money += MONEY_REWARD;
                    _catSprite._textMoney.text = _catSprite.money.ToString();
                    _animator.SetInteger("state", 2);
                    isEmpty = true;
                    icon.enabled = false;
                    textIcon.enabled = false;
                }
                _poly[0].enabled = true;
                _poly[1].enabled = false;
            }
        }
        if (Vector3.Distance(_cat.transform.position-new Vector3(0,0.4f,0), 
                !isOpened ? transform.position : transform.position + new Vector3(0.35f,0,0)) < 1.3 && (!isOpened || !isEmpty))
        {
            icon.enabled = true;
            textIcon.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                _audioOpenSource.Play();
                isOpened = true;
                icon.transform.position += new Vector3(0.5f, 0f, 0f);
            }
        }
        else
        {
            icon.enabled = false;
            textIcon.enabled = false;
        }
    }

    public void Save()
    {
        if (this != null) 
            SavingSystem<Chest,ChestData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        _data = SavingSystem<Chest, ChestData>.Load($"{gameObject.name}.data");
        _animator.SetInteger("state", _data.animatorState);
        isOpened = _data.isOpened;
        isEmpty = _data.isEmpty;
    }
}