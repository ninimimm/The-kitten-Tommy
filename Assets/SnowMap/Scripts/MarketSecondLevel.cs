using System;
using TMPro;
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
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.enabled = false;
        _catSprite = cat.GetComponent<CatSprite>();
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
        if (Math.Abs(transform.position.x - _catTransform.position.x) < 1 && _catTransform.position.y - transform.position.y < 2
            && _catTransform.position.y - transform.position.y > 0 && _catSprite.isWater)
        {
            icon.enabled = true;
            textIcon.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
            if (_spriteRenderer.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 6)
                {
                    _catSprite.money -= 6;
                    _catSprite.knife.knife.GetComponent<SpriteRenderer>().sprite = PoisonKnife;
                    _catSprite.isPoison = true;
                    buyingSourse.Play();
                    market1.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 4)
                {
                    _catSprite.money -= 4;
                    _catSprite.countHealth += 1;
                    _catSprite._textHealth.text = _catSprite.countHealth.ToString();
                    buyingSourse.Play();
                    market2.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 3)
                {
                    _catSprite.money -= 3;
                    _catSprite.HP = _catSprite.maxHP;
                    _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
                    buyingSourse.Play();
                    market3.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 3)
                {
                    _catSprite.money -= 3;
                    _catSprite.takeDamage += clawsDamageUpgrade;
                    buyingSourse.Play();
                    market4.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 2)
                {
                    grabbingHook.enabled = true;
                    if (grabbingHook.material != ColdHarpoon)
                    {
                        _catSprite.money -= 2;
                        grabbingHook.material = ColdHarpoon;
                        line.material = "cold";
                    }
                    grabbingHook.enabled = false;
                    buyingSourse.Play();
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
