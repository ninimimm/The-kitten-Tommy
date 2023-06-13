using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSecondLevle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask balloonLayer;
    [SerializeField] private ComponentSnake snake;
    [SerializeField] private BirdIdle _birdIdle;
    [SerializeField] private Scorpio[] scorpios;
    [SerializeField] private Hawk _hawk;
    [SerializeField] private BirdStay _birdStay;
    [SerializeField] private EatingBird[] eatingBirds;
    [SerializeField] private Chest chest;
    [SerializeField] private Hyena hyena;
    [SerializeField] private Line grabbingHook;
    [SerializeField] private TakeKey takeKey;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    [SerializeField] private Boosts boosts;
    public static SandBoss boss;
    public static int countCoins;
    public static List<Coin> coins = new ();
    public static List<Crate> crates = new ();
    public static List<Mummy> mummies = new();
    private CatSprite _catSprite;

    void Start()
    {
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_catSprite.isBossDead && 
            Physics2D.OverlapCircle(_catSprite.groundCheck.position, _catSprite.groundCheckRadius, balloonLayer))
        {
            transform.position += speed * Time.deltaTime * movingVector ;
            _cat.transform.position += speed * Time.deltaTime * movingVector ;
        }

        if (transform.position.y > 5)
        {
            Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    private void Save()
    {
        foreach (var coin in coins)
            if (coin is not null) coin.Save();
        _cat.Save();
        foreach (var crate in crates)
            if (crate is not null) crate.Save();
        snake.Save();
        _birdIdle.Save();
        foreach (var scorpio in scorpios)
            scorpio.Save();
        _hawk.Save();
        _birdStay.Save();
        foreach (var eatingBird in eatingBirds)
            eatingBird.Save();
        chest.Save();
        hyena.Save();
        boss.Save();
        foreach (var mummy in mummies)
            if (mummy is not null) mummy.Save();
        grabbingHook.Save();
        takeKey.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
    }
}
