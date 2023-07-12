using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MarketThirdLevel : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private LineRenderer grabbingHook;
    [SerializeField] private Material treeHarpoon;
    [SerializeField] private Line line;
    [SerializeField] private AudioSource buyingSourse;
    [SerializeField] public SpriteRenderer icon;
    [SerializeField] public TextMeshProUGUI textIcon;
    [SerializeField] private Boosts boosts;
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
            && _catTransform.position.y - transform.position.y > 0)
        {
            icon.enabled = true;
            textIcon.enabled = true;
            if(Input.GetKeyDown(KeyCode.E))
                _spriteRenderer.enabled = !_spriteRenderer.enabled;
            if (_spriteRenderer.enabled)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 2)
                {
                    _catSprite.money -= 2;
                    boosts.timeToRun += 0.5f;
                    buyingSourse.Play();
                    market1.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 5)
                {
                    _catSprite.money -= 5;
                    _catSprite.countHealth += 1;
                    _catSprite._textHealth.text = _catSprite.countHealth.ToString();
                    buyingSourse.Play();
                    market2.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 4)
                {
                    _catSprite.money -= 4;
                    _catSprite.HP = _catSprite.maxHP;
                    _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
                    buyingSourse.Play();
                    market3.enabled = true;
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 2)
                {
                    _catSprite.money -= 2;
                    boosts.timeToJump += 0.5f;
                    buyingSourse.Play();
                    market4.enabled = true; 
                    timer = timeWait;
                }
                else if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 4)
                {
                    grabbingHook.enabled = true;
                    if (grabbingHook.material != treeHarpoon)
                    {
                        _catSprite.money -= 4;
                        grabbingHook.material = treeHarpoon;
                        line.material = "tree";
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
