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
    public Animator _animator;
    public PolygonCollider2D[] _poly;
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
        _poly = GetComponents<PolygonCollider2D>();
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
        if (Vector3.Distance(_cat.transform.position-new Vector3(0,0.4f,0), transform.position) < 1.3 && !isOpened)
        {
            icon.enabled = true;
            textIcon.enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                _audioOpenSource.Play();
                isOpened = true;
            }
        }
        else
        {
            icon.enabled = false;
            textIcon.enabled = false;
        }
        
        if (!isEmpty)
        {
            if (isOpened)
            {
                icon.enabled = false;
                textIcon.enabled = false;
                _animator.SetInteger("state", 1);
                if (Vector3.Distance(_cat.transform.position, transform.position) < 0.85)
                {
                    _audioMoneySource.Play();
                    _catSprite.money += MONEY_REWARD;
                    _catSprite._textMoney.text = _catSprite.money.ToString();
                    _animator.SetInteger("state", 2);
                    isEmpty = true;
                }
                _poly[0].enabled = true;
                _poly[1].enabled = false;
            }
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