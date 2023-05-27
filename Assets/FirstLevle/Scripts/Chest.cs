using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    private const int MONEY_REWARD = 5;
    [SerializeField] private GameObject _cat;
    [SerializeField] float distanseAttack;
    [SerializeField] private LayerMask catLayer;
    [SerializeField] private AudioSource _audioOpenSource;
    private enum MovementState { close, opened, empty};
    private MovementState _stateChest;
    public Animator _animator;
    public PolygonCollider2D[] _poly;
    public bool isOpened = false;
    public bool haveMoney = true;
    private CatSprite _catSprite;
    private ChestData _data;
    private bool isStart = true;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _poly = GetComponents<PolygonCollider2D>();
        _catSprite = _cat.GetComponent<CatSprite>();
        _poly[1].enabled = true;
        _poly[0].enabled = false;
        _animator.SetInteger("state", 0);
        if (!ChestData.start.Contains(gameObject.name))
        {
            ChestData.start.Add(gameObject.name);
            Save();
        }
    }

    void Update()
    {
        var isEmpty = _animator.GetCurrentAnimatorStateInfo(0).IsName("empty");
        if (!isEmpty)
        {
            var catTouch = Physics2D.OverlapCircleAll(transform.position, distanseAttack, catLayer);
            if (isOpened)
            {
                _animator.SetInteger("state", 1);
                if (catTouch.Length > 0)
                {
                    _catSprite.money += MONEY_REWARD;
                    _animator.SetInteger("state", 2);
                }
                _poly[0].enabled = true;
                _poly[1].enabled = false;
            }
        }

        if (isStart)
        {
            Load();
            isStart = false;
        }
    }

    public void TakeDamage(float damage) => isOpened = true;

    public void Save()
    {
        SavingSystem<Chest,ChestData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        _data = SavingSystem<Chest, ChestData>.Load($"{gameObject.name}.data");
        isOpened = _data.isOpened;
        _animator.SetInteger("state", _data.animatorState);
    }
}