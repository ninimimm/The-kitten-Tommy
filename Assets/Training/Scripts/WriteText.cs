using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WriteText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject _cat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_cat.transform.position.x < -21)
            text.text = "Для движения используйте AD или стрелочки";
        else if (_cat.transform.position.x > -21 && _cat.transform.position.x < -16)
            text.text = "Для прыжка нажмите пробел";
        else if (_cat.transform.position.x > -16 && _cat.transform.position.x < -13)
            text.text = "Чтобы выпустить крюк, наведитесь мышкой на место и нажмите Shift";
        else if (_cat.transform.position.x > -13 && _cat.transform.position.x < -11)
            text.text = "Крюк имеет область действия, почувствуйте ее\n"+"Чтобы повиснуть на расстоянии над землей, выпустите крюк в прыжке";
        else if (_cat.transform.position.x > -11 && _cat.transform.position.x < -5)
            text.text = "Попытайтесь перелететь через яму с помощью крюка";
        else if (_cat.transform.position.x > -5 && _cat.transform.position.x < -1)
            text.text = "Чтобы ударить змею нажмите Q или киньте кинжал нажав лкм и указав мышью направление";
        else if (_cat.transform.position.x > -1 && _cat.transform.position.x < 6)
            text.text = "Попытайтесь пользуясь крюком перелететь на блок выше";
        else if (_cat.transform.position.x > 6 && _cat.transform.position.x < 7)
            text.text = "Для сохранения нажмите CapsLock у таблички";
        else if (_cat.transform.position.x > 7 && _cat.transform.position.x < 8)
            text.text = "Попытайтесь пользуясь крюком перелететь на блок выше";
        else if (_cat.transform.position.x > 8 && _cat.transform.position.x < 14)
            text.text = "Перелетите на платформу, находящуюся на воздухе";
        else if (_cat.transform.position.x > 14 && _cat.transform.position.x < 16)
            text.text = "Ударьте сундук с деньгами когтями чтобы открыть и подойдите чтобы собрать монетки";
        else if (_cat.transform.position.x > 16 && _cat.transform.position.x < 20)
            text.text = "Нажмите E чтобы открыть сундук с ключом и снова E, чтобы его взять";
        else if (_cat.transform.position.x > 20 && _cat.transform.position.x < 30)
            text.text = "Научитесь перехватываться крюком, чтобы забираться выше на платформы";
        else if (_cat.transform.position.x > 30 && _cat.transform.position.x < 40)
            text.text = "Крюком можно стягивать коробки, чтобы забираться по ним на платформы";
        else if (_cat.transform.position.x > 40 && _cat.transform.position.x < 44)
            text.text = "Крюком можно стягивать коробки, чтобы забираться по ним на платформы";
        else if (_cat.transform.position.x > 44 && _cat.transform.position.x < 60)
            text.text = "Коробки можно закидывать на платформы, сделайте это, чтобы забраться на самую последнюю платформу";
    }
}
