using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnifeBar : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private Gradient _gradient;
    [SerializeField] private Image _colorBar;

    public void SetMaxHealth(float maxTime)
    {
        (_slider.maxValue, _slider.value) = (maxTime, maxTime);
        _colorBar.color = _gradient.Evaluate(1f);
    }

    public void SetHealth(float time)
    { 
        _slider.value = time;
        _colorBar.color = _gradient.Evaluate(_slider.normalizedValue);
    }
}