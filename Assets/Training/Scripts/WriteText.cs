using JetBrains.Annotations;
using UnityEngine;

public class WriteText : MonoBehaviour
{
    [SerializeField] private SpriteRenderer A;
    [SerializeField] private SpriteRenderer D;
    [SerializeField] private SpriteRenderer Spase;
    [SerializeField] public GameObject snakeTarget;
    [SerializeField] public GameObject scorpioTarget;
    [SerializeField] public GameObject LKM;
    [SerializeField] public SpriteRenderer W;
    [SerializeField] public GameObject scorpoi3;
    [SerializeField] public SpriteRenderer platformTarget;
    [SerializeField] private SpriteRenderer PKM1;
    [SerializeField] private GrabbingHook _grabbing;
    [SerializeField] public GameObject crateTarget;
    [SerializeField] public SpriteRenderer PKM2;
    [SerializeField] public SpriteRenderer Spase2;
    [SerializeField] public SpriteRenderer plus;
    [SerializeField] public SpriteRenderer A2;
    [SerializeField] public SpriteRenderer D2;
    [SerializeField] public SpriteRenderer mouseWheelUp;
    [SerializeField] public SpriteRenderer mouseWheelDown;
    [SerializeField] public SpriteRenderer LKM2;


    [SerializeField] private GameObject _cat;
    private GrabbingHook _grabbingHook;

    private bool isMove;
    private bool isJump;
    private bool isHook;
    private bool isAttack;
    public bool isBreake;
    public bool firstTime1 = true;
    public bool firstTime2 = true;
    public bool firstTime3 = true;
    public bool firstTime4 = true;
    public bool final;
    public bool isWeel;
    // Start is called before the first frame update
    void Start()
    {
        crateTarget.GetComponent<SpriteRenderer>().enabled = false;
        PKM2.enabled = false;
        _grabbingHook = _cat.GetComponent<GrabbingHook>();
    }

    // Update is called once per frame
    void Update()
    {
        LKM.transform.position = scorpoi3.transform.position-new Vector3(0,0.5f,0);
        snakeTarget.transform.Rotate(0,0,0.5f);
        scorpioTarget.transform.Rotate(0,0,0.5f);
        platformTarget.transform.Rotate(0,0,0.5f);
        platformTarget.transform.Rotate(0,0,0.5f);
        LKM2.transform.position = new Vector3(crateTarget.transform.position.x, LKM2.transform.position.y, LKM2.transform.position.z);
        if (Input.GetKeyDown(KeyCode.A))
            A.enabled = false;
        if (Input.GetKeyDown(KeyCode.D))
            D.enabled = false;
        if (!A.enabled && !D.enabled) isMove = true;
        
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spase.enabled = false;
            isJump = true;
        }
        
        if (_grabbing.isHookedStatic && firstTime1 && !_cat.GetComponent<CatSprite>().isGround && !final)
        {
            firstTime1 = false;
            PKM1.enabled = false;
            platformTarget.GetComponent<SpriteRenderer>().enabled = false;
            Spase2.enabled = false;
            plus.enabled = false;
            PKM2.enabled = true;
            A2.enabled = true;
            D2.enabled = true;
            crateTarget.GetComponent<SpriteRenderer>().enabled = true;
        }

        if (!firstTime1)
        {
            if (Input.GetKeyDown(KeyCode.A))
                A2.enabled = false;
            else if (Input.GetKeyDown(KeyCode.D))
                D2.enabled = false;
            if (!Spase2.enabled && !A2.enabled && !D2.enabled && firstTime3)
            {
                firstTime3 = false;
                mouseWheelUp.enabled = true;
            }
            if (!firstTime3 && !mouseWheelUp.enabled && !Spase2.enabled && !A2.enabled && !D2.enabled && firstTime4)
            {
                firstTime4 = false;
                mouseWheelDown.enabled = true;
            }
            if (!_grabbing.isHookedStatic)
            {
                firstTime1 = true;
                A2.enabled = false;
                D2.enabled = false;
                mouseWheelUp.enabled = false;
                mouseWheelDown.enabled = false;
                PKM1.enabled = true;
                platformTarget.GetComponent<SpriteRenderer>().enabled = true;
                Spase2.enabled = true;
                plus.enabled = true;
                firstTime3 = true;
                firstTime4 = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && !PKM2.enabled)
        {
            LKM2.enabled = false;
            crateTarget.GetComponent<SpriteRenderer>().enabled = false;
        }

        if (final)
        {
            PKM1.enabled = false;
            Spase2.enabled = false;
            plus.enabled = false;
            platformTarget.GetComponent<SpriteRenderer>().enabled = false;
        }
    }
}