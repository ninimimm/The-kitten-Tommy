using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class MarketSecondLevel : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private int clawsDamageUpgrade;
    [SerializeField] private LineRenderer grabbingHook;
    [SerializeField] private Material ColdHarpoon;
    [SerializeField] private Sprite PoisonKnife;
    [SerializeField] private GameObject _knifePrefab;
    [SerializeField] private Line line;
    private SpriteRenderer _knifeSpriteRenderer;
    private Transform _catTransform;
    private SpriteRenderer _spriteRenderer;
    private CatSprite _catSprite;
    void Start()
    {
        _catTransform = cat.GetComponent<Transform>();
        _knifeSpriteRenderer = _knifePrefab.GetComponent<SpriteRenderer>();
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
            if (Input.GetKeyDown(KeyCode.Alpha1) && _catSprite.money >= 6 && _knifeSpriteRenderer.sprite != PoisonKnife)
            {
                _catSprite.money -= 6;
                _knifeSpriteRenderer.sprite = PoisonKnife;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _catSprite.money >= 4)
            {
                _catSprite.money -= 4;
                _catSprite.countHealth += 1;
                _catSprite._textHealth.text = _catSprite.countHealth.ToString();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3) && _catSprite.money >= 3)
            {
                _catSprite.money -= 3;
                _catSprite.HP = _catSprite.maxHP;
                _catSprite._healthBar.SetMaxHealth(_catSprite.maxHP);
            }
            if (Input.GetKeyDown(KeyCode.Alpha4) && _catSprite.money >= 3)
            {
                _catSprite.money -= 3;
                _catSprite.takeDamage += clawsDamageUpgrade;
            }
            if (Input.GetKeyDown(KeyCode.Alpha5) && _catSprite.money >= 6)
            {
                grabbingHook.enabled = true;
                if (grabbingHook.material != ColdHarpoon)
                {
                    _catSprite.money -= 6;
                    grabbingHook.material = ColdHarpoon;
                    line.material = "cold";
                }
                grabbingHook.enabled = false;
            }
        }
        else
        {
            _spriteRenderer.enabled = false;
        }
    }
}
