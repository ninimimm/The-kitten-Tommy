using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSecondLevle : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject _cat;
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
    public static int countCoins;
    public static List<GameObject> coins = new ();
    public static List<GameObject> crates = new ();
    private CatSprite _catSprite;
    
    
    
    
    
    void Start()
    {
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircleAll(_catSprite.groundCheck.position, _catSprite.groundCheckRadius, balloonLayer).Length > 0
            && _catSprite.isBossDead)
        {
            transform.position += movingVector * speed * Time.deltaTime;
            _cat.transform.position += movingVector * speed * Time.deltaTime;
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
            coin.GetComponent<Coin>().Save();
        _cat.GetComponent<CatSprite>().Save();
        foreach (var crate in crates)
            crate.GetComponent<Crate>().Save();
        snake.Save();
        _birdIdle.GetComponent<BirdIdle>().Save();
        foreach (var scorpio in scorpios)
            scorpio.Save();
        _hawk.Save();
        _birdStay.Save();
        foreach (var eatingBird in eatingBirds)
            eatingBird.GetComponent<EatingBird>().Save();
        chest.Save();
        hyena.Save();
    }
}
