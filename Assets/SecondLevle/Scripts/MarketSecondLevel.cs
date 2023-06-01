using System;
using UnityEngine;

public class MarketSecondLevel : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private int clawsDamageUpgrade;
    [SerializeField] private LineRenderer grabbingHook;
    [SerializeField] private Material ColdHarpoon;
    [SerializeField] private Sprite PoisonKnife;
    [SerializeField] private Line line;
    [SerializeField] private AudioSource buyingSourse;
    private Transform _catTransform;
    private SpriteRenderer _spriteRenderer;
    private CatSprite _catSprite;
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _catSprite = cat.GetComponent<CatSprite>();
    }
    
    void Update()
    {
        if (Math.Abs(transform.position.x - _catTransform.position.x) < 1 && _catTransform.position.y - transform.position.y < 2
            && _catTransform.position.y - transform.position.y > 0 && _catSprite.isWater)
        {
            _spriteRenderer.enabled = true;
            if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 6)
            {
                _catSprite.money -= 6;
                _catSprite.knife.knife.GetComponent<SpriteRenderer>().sprite = PoisonKnife;
                _catSprite.isPoison = true;
                buyingSourse.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 4)
            {
                _catSprite.money -= 4;
                _catSprite.countHealth += 1;
                _catSprite._textHealth.text = _catSprite.countHealth.ToString();
                buyingSourse.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 3)
            {
                _catSprite.money -= 3;
                _catSprite.HP = _catSprite.maxHP;
                _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
                buyingSourse.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 3)
            {
                _catSprite.money -= 3;
                _catSprite.takeDamage += clawsDamageUpgrade;
                buyingSourse.Play();
            }
            else if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 6)
            {
                grabbingHook.enabled = true;
                if (grabbingHook.material != ColdHarpoon)
                {
                    _catSprite.money -= 6;
                    grabbingHook.material = ColdHarpoon;
                    line.material = "cold";
                }
                grabbingHook.enabled = false;
                buyingSourse.Play();
            }
            _catSprite._textMoney.text = _catSprite.money.ToString();
        }
        else
            _spriteRenderer.enabled = false;
    }
}
