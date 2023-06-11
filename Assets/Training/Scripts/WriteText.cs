using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WriteText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    [SerializeField] private GameObject _cat;
    private GrabbingHook _grabbingHook;

    private bool isMove;
    private bool isJump;
    private bool isHook;
    private bool isAttack;
    // Start is called before the first frame update
    void Start()
    {
        _grabbingHook = _cat.GetComponent<GrabbingHook>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMove)
        {
            text.text = "Для движения используйте AD или стрелочки";
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) isMove = true;
        }
        else if (!isJump)
        {
            text.text = "Для прыжка нажмите пробел";
            if (Input.GetKeyDown(KeyCode.Space)) isJump = true;
        }
        else if (!isAttack)
        {
            text.text = "Чтобы ударить врага когтями нажмите W или киньте кинжал нажав ЛКМ, указав мышью направление";
            if (Input.GetKeyDown(KeyCode.Mouse0)) isAttack = true;
        }
        else if (!isHook)
        {
            text.text = "Чтобы выпустить крюк, наведитесь мышкой на землю, коробку или платформу и нажмите ПКМ";
            if (_grabbingHook.isHookedDynamic || _grabbingHook.isHookedStatic)
            {
                isHook = true;
                text.text = "Крюк имеет область действия\n"+"Чтобы повиснуть на расстоянии над землей, выпустите крюк в прыжке";
            }
        }
    }
}
