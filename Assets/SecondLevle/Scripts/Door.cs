using System;
using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private GameObject cat;
    [SerializeField] private string key;
    [SerializeField] private GameObject logicKeys;
    [SerializeField] public AudioSource openDoor;
    [SerializeField] private SpriteRenderer spriteE;
    [SerializeField] private TextMeshProUGUI textE;
    [SerializeField] private AudioSource knock;
    private TakeKey _takeKey;
    public BoxCollider2D boxCollider2D;
    public Animator animator;
    private CatSprite _catSprite;
    private DoorData _data;
    void Start()
    {
        _takeKey = logicKeys.GetComponent<TakeKey>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        _catSprite = cat.GetComponent<CatSprite>();
        animator = GetComponent<Animator>();
        if (!DoorData.Start.Contains(gameObject.name))
        {
            DoorData.Start.Add(gameObject.name);
            Save();
        }
        Load();
    }
    
    void Update()
    {
        if (Math.Abs(transform.position.x - cat.transform.position.x) < 1 && animator.GetInteger("state") == 0)
        {
            spriteE.enabled = true;
            textE.enabled = true;
        }
        else
        {
            spriteE.enabled = false;
            textE.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && Math.Abs(transform.position.x - cat.transform.position.x) < 1 && _catSprite.key == key)
        {
            openDoor.Play();
            animator.SetInteger("state", 1);
            _catSprite.key = "";
            boxCollider2D.enabled = false;
            if (key == "iron") _takeKey.ironKeyImage.enabled = false;
            else _takeKey.goldKeyImage.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.E) && Math.Abs(transform.position.x - cat.transform.position.x) < 1 && !knock.isPlaying)
            knock.Play();
    }
    
    public void Save()
    {
        SavingSystem<Door,DoorData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        _data = SavingSystem<Door, DoorData>.Load($"{gameObject.name}.data");
        animator.SetInteger("state", _data.animatorState);
        boxCollider2D.enabled = _data.colliderState;
    }
}