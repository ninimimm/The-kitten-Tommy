using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private CatSprite cat;
    [SerializeField] private float timeLite;
    private CircleCollider2D _circleCol;
    public float timer = -0.1f;
    private SpriteRenderer _image;
    private int _countLite;
    private float _timerChose;
    void Start()
    {
        _circleCol = GetComponent<CircleCollider2D>();
        _image = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        transform.position = new Vector3(cat.transform.position.x,cat.transform.position.y-0.45f);
        if (timer >= 0)
        {
            cat.canTakeDamage = false;
            timer -= Time.deltaTime;
            _image.enabled = true;
            _circleCol.enabled = true;
        }
        else if (_countLite < 4 && !cat.canTakeDamage)
        {
            if (_timerChose >= 0)
            {
                _timerChose -= Time.deltaTime;
                if (_timerChose < timeLite / 2)_image.enabled = true;
            }
            else
            {
                _countLite++;
                _image.enabled = false;
                _timerChose = timeLite;
            }
        }
        else if (_countLite == 4)
        {
            cat.canTakeDamage = true;
            _countLite = 0;
            _image.enabled = false;
            _circleCol.enabled = false;
        }
    }
}
