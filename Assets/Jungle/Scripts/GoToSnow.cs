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
        if (Physics2D.OverlapCircle(_catSprite.groundCheck.position, _catSprite.groundCheckRadius, balloonLayer))
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

    private void Save()
    {
        foreach (var coin in coins)
            if (coin is not null) coin.Save();
        _cat.Save();
        foreach (var crate in crates)
            if (crate is not null) crate.Save();
        chest.Save();
        grabbingHook.Save();
        takeKey.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
        foreach (var people in peoples)
            people.Save();
        foreach (var door in doors)
            if (door is not null) door.Save();
    }
}
