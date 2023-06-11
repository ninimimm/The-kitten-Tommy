using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextInTable : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
    [SerializeField] public SpriteRenderer icon;
    [SerializeField] public TextMeshProUGUI textIcon;
    private Text text;
    public bool inputE;
    // Start is called before the first frame update
    void Start()
    {
        icon.enabled = false;
        textIcon.enabled = false;
        text = GetComponent<Text>();
        text.text = "Послышился какой-то шорох\n" +
                    "Что это?";
    }

    // Update is called once per frame
    void Update()
    {
        if (_cat.transform.position.x > 9.1)
        {
            icon.enabled = true;
            textIcon.enabled = true;
            text.text = "Хотите заглянуть?";
        }

        if (inputE)
        {
            icon.enabled = false;
            textIcon.enabled = false;
            text.text = "Кажется, это место давно заброшено\n4 1 5 2 3 ...";
        }
    }
}
