using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WriteText : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI text;
    [SerializeField] public Image learning;

    [SerializeField] private GameObject _cat;
    private GrabbingHook _grabbingHook;

    private bool isMove;
    private bool isJump;
    private bool isHook;
    private bool isAttack;
    public bool isBreake;
    public bool firstTime = true;
    public bool isWeel;
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
                isHook = true;
        }
        else if (!isWeel)
        {
            text.text = "Чтобы подниматься или попускаться на крюке, ипользуйте колесико мыши";
            if (Input.GetAxis("Mouse ScrollWheel") > 0) isWeel = true;
        }
        else if (!isBreake)
        {
            text.text = "Вы можете сломать коробку и получить бонус, выпустив в нее кинжал и скинув на землю";
        }
    }
}
