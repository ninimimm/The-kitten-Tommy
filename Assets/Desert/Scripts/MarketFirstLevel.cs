using System;
using TMPro;
using UnityEngine;

public class MarketFirstLevel : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private float knifeIntervalUpgrade;
    [SerializeField] private float knifeDamageUpgrade;
    [SerializeField] private LineRenderer grabbingHook;
    [SerializeField] private Material GoldHarpoon;
    [SerializeField] private GameObject _knifePrefab;
    [SerializeField] private Line line;
    [SerializeField] private Knife _knife;
    [SerializeField] private AudioSource buyingSource;
    [SerializeField] public SpriteRenderer icon;
    [SerializeField] public TextMeshProUGUI textIcon;
    [SerializeField] private float timeWait;
    [SerializeField] private SpriteRenderer market1;
    [SerializeField] private SpriteRenderer market2;
    [SerializeField] private SpriteRenderer market3;
    [SerializeField] private SpriteRenderer market4;
    [SerializeField] private SpriteRenderer market5;
    private float timer;
    private Transform _catTransform;
    private SpriteRenderer _spriteRenderer;
    private CatSprite _catSprite;
    private logicKnife _logicKnife;

    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _catSprite = cat.GetComponent<CatSprite>();
        _logicKnife = _knifePrefab.GetComponent<logicKnife>();
        icon.enabled = false;
        textIcon.enabled = false;
    }
    
    void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;
        else
        {
            market1.enabled = false;
            market2.enabled = false;
            market3.enabled = false;
            market4.enabled = false;
            market5.enabled = false;
        }
        if (Math.Abs(transform.position.x - _catTransform.position.x) < 1 && Math.Abs(transform.position.y - _catTransform.position.y) < 1.5
                                                                          && !_catSprite.isWater)
        {
            icon.enabled = true;
            textIcon.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
            if (_spriteRenderer.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 2)
                {
                    buyingSource.Play();
                    _catSprite.money -= 2;
                    _knife.attackIntervale *= knifeIntervalUpgrade;
                    _catSprite._knifeBar.SetMaxHealth(_knife.attackIntervale);
                    market1.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 3)
                {
                    buyingSource.Play();
                    _catSprite.money -= 3;
                    _catSprite.countHealth += 1;
                    _catSprite._textHealth.text = _catSprite.countHealth.ToString();
                    market2.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 2)
                {
                    buyingSource.Play();
                    _catSprite.money -= 2;
                    _catSprite.HP = _catSprite.maxHP;
                    _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
                    market3.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 1)
                {
                    buyingSource.Play();
                    _catSprite.money -= 1;
                    _logicKnife.damage += knifeDamageUpgrade;
                    market4.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 1)
                {
                    buyingSource.Play();
                    grabbingHook.enabled = true;
                    if (grabbingHook.material != GoldHarpoon)
                    {
                        _catSprite.money -= 1;
                        grabbingHook.material = GoldHarpoon;
                        line.material = "gold";
                    }
                    grabbingHook.enabled = false;
                    market5.enabled = true;
                    timer = timeWait;
                }
                _catSprite._textMoney.text = _catSprite.money.ToString();
            }
        }
        else
        {
            icon.enabled = false;
            textIcon.enabled = false;
            _spriteRenderer.enabled = false;
        }
            
    }
}
