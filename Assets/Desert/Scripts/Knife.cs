using UnityEngine;
using UnityEngine.SceneManagement;

public class Knife : MonoBehaviour
{
    [SerializeField] private CatSprite catSprite;
    [SerializeField] public float attackIntervale;
    [SerializeField] private PoisonKnife _poisonKnife;
    [SerializeField] private AudioSource flySource;
    [SerializeField] private AudioSource hitSource;
    public Transform fireKnife;
    public GameObject knife;
    public float timer;
    private bool nowIsShoot;
    private KnifeData data;
    private GameObject newKnife;
    private Vector3 mouseWorldPosition;
    private Vector2 direction;
    private Camera mainCamera;
    private float angle;
    private int index = -1;
    
    private void Start()
    {
        data = SavingSystem<Knife, KnifeData>.Load($"{gameObject.name}.data");
        mainCamera = Camera.main;
        if (!MainMenu.dictSave.ContainsKey(gameObject.name))
        {
            MainMenu.dictSave.Add(gameObject.name,MainMenu.index);
            MainMenu.index ++;
        }
        if (MainMenu.isStarts[MainMenu.dictSave[gameObject.name]])
        {
            Save();
            MainMenu.isStarts[MainMenu.dictSave[gameObject.name]] = false;
        }
        Load();
        timer = attackIntervale;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > attackIntervale)
            nowIsShoot = false;
        if (nowIsShoot) timer += Time.deltaTime;
        else if (Input.GetButtonDown("Fire1") && !catSprite.inMiniMap)
        {
            timer = 0;
            Shoot();
            nowIsShoot = true;
        }
    }
    
    public void Save()
    {
        if (this != null) 
            SavingSystem<Knife,KnifeData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Knife, KnifeData>.Load($"{gameObject.name}.data");
        attackIntervale = data.attackIntervale;
        catSprite._knifeBar.SetMaxHealth(attackIntervale);
    }

    void Shoot()
    {
        newKnife = Instantiate(knife, fireKnife.position, fireKnife.rotation);
        newKnife.GetComponent<logicKnife>().catSprite = catSprite;
        newKnife.GetComponent<logicKnife>()._poisonKnife = _poisonKnife;
        newKnife.GetComponent<logicKnife>().flySource = flySource;
        newKnife.GetComponent<logicKnife>().hitSource = hitSource;
        mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.nearClipPlane));
        direction = (new Vector2(mouseWorldPosition.x, mouseWorldPosition.y) - (Vector2)fireKnife.position).normalized;
        newKnife.GetComponent<Rigidbody2D>().velocity = direction;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        newKnife.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
