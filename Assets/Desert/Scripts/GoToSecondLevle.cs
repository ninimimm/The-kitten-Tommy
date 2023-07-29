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
    private float timer = 3;

    void Start()
    {
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0 && _catSprite.isBossDead &&
            (Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius, balloonLayer) ||
             Physics2D.OverlapCircle(_catSprite.groundCheck2.position, _catSprite.groundCheckRadius, balloonLayer)))
        {
            transform.position += speed * Time.deltaTime * movingVector;
            _cat.transform.position += speed * Time.deltaTime * movingVector;
        }
        else timer -= Time.deltaTime;

        if (transform.position.y > 5)
        {
            Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void Save()
    {
        foreach (var coin in coins)
            coin.Save();
        _cat.Save();
        foreach (var crate in crates)
            crate.Save();
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
        if (boss != null)
            boss.Save();
        foreach (var mummy in mummies)
            mummy.Save();
        grabbingHook.Save();
        takeKey.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
    }
}
