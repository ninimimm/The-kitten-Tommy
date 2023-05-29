using System;
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
    }
    
    void Update()
    {
        if (Math.Abs(transform.position.x - _catTransform.position.x) < 1 && Math.Abs(transform.position.y - _catTransform.position.y) < 1.5
            && !_catSprite.isWater)
        {
            _spriteRenderer.enabled = true;
            if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 2)
            {
                _catSprite.money -= 2;
                _knife.attackIntervale *= knifeIntervalUpgrade;
                _catSprite._knifeBar.SetMaxHealth(_knife.attackIntervale);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 3)
            {
                _catSprite.money -= 3;
                _catSprite.countHealth += 1;
                _catSprite._textHealth.text = _catSprite.countHealth.ToString();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 2)
            {
                _catSprite.money -= 2;
                _catSprite.HP = _catSprite.maxHP;
                _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 1)
            {
                _catSprite.money -= 1;
                _logicKnife.damage += knifeDamageUpgrade;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 3)
            {
                grabbingHook.enabled = true;
                if (grabbingHook.material != GoldHarpoon)
                {
                    _catSprite.money -= 3;
                    grabbingHook.material = GoldHarpoon;
                    line.material = "gold";
                }
                grabbingHook.enabled = false;
            }
            _catSprite._textMoney.text = _catSprite.money.ToString();
        }
        else
            _spriteRenderer.enabled = false;
    }
}
