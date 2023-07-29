using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSnow : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask balloonLayer;
    [SerializeField] private Chest chest;
    [SerializeField] private Line grabbingHook;
    [SerializeField] private TakeKey takeKey;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    [SerializeField] private Boosts boosts;
    [SerializeField] private List<People> peoples;
    [SerializeField] private Door[] doors;
    [SerializeField] private Slime[] slimes;
    public static int countCoins;
    public static List<Coin> coins = new ();
    public static List<Crate> crates = new ();
    private CatSprite _catSprite;

    void Start()
    {
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius, balloonLayer) ||
            Physics2D.OverlapCircle(_catSprite.groundCheck2.position, _catSprite.groundCheckRadius, balloonLayer))
        {
            transform.position += speed * Time.deltaTime * movingVector ;
            _cat.transform.position += speed * Time.deltaTime * movingVector ;
        }

        if (transform.position.y > 8)
        {
            Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    public void Save()
    {
        foreach (var coin in coins) coin.Save();
        _cat.Save();
        foreach (var crate in crates) crate.Save();
        chest.Save();
        grabbingHook.Save();
        takeKey.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
        foreach (var people in peoples) people.Save();
        foreach (var door in doors) door.Save();
        foreach (var slime in slimes) slime.Save();
    }
}
