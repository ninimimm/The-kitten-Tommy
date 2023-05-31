using UnityEngine;
using UnityEngine.UI;

public class TextInTable : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
    private Text text;
    public bool inputE;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        text.text = "Послышился какой-то шорох\n" +
                    "Что это?";
    }

    // Update is called once per frame
    void Update()
    {
        if (_cat.transform.position.x > 9.1)
            text.text = "Хотите заглянуть?\nНажмите E";
        if (inputE)
            text.text = "Кажется, это место давно заброшено\n4 1 5 2 3 ...";
    }
}
