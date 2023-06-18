using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HighlightOnSelect : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Color normalColor = Color.white; // Замените на ваш нормальный цвет
    public Color selectedColor = Color.white; // Замените на цвет при выборе

    private Image buttonImage;

    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        buttonImage.color = selectedColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        buttonImage.color = normalColor;
    }
}