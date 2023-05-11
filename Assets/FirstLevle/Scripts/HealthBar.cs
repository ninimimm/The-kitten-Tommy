using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _colorBar;

    public void SetMaxHealth(float maxHp)
    {
        (_slider.maxValue, _slider.value) = (maxHp, maxHp);
        _colorBar.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(float hp)
    { 
        _slider.value = hp;
        _colorBar.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}
