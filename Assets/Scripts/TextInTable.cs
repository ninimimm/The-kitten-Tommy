using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextInTable : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
    private Text text;
    private bool broke;
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
            text.text = "Хотите заглянуть?\nНажмите пкм";
        if (Input.GetKey(KeyCode.Mouse1))
            broke = true;
        if (broke)
            text.text = "Кажется, это место давно заброшено";
    }
}
