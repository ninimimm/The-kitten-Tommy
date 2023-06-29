using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToFirstLevel : MonoBehaviour
{
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask balloonLayer;
    [SerializeField] private Scorpio[] scorpios;
    [SerializeField] private Chest chest;
    [SerializeField] private Line grabbingHook;
    [SerializeField] private TakeKey takeKey;
    [SerializeField] private ManageButtons manageButtons;
    [SerializeField] private Door[] doors;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    [SerializeField] private Boosts boosts;
    public static List<Coin> coins = new ();
    public static List<Crate> crates = new ();
    public static int countCoins;
    private CatSprite _catSprite;
    
    
    void Start()
    {
        _catSprite = _cat.GetComponent<CatSprite>();
    }

    void Update()
    {
        if ((Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius, balloonLayer) ||
             Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius, balloonLayer)) &&
            Vector3.Distance(_catSprite.transform.position,transform.position) < 5)
        {
            transform.position += speed * Time.deltaTime * movingVector ;
            _cat.transform.position += speed * Time.deltaTime * movingVector ;
        }

        if (transform.position.y > 10)
        {
            Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
            
    }
    public void Save()
    {
        foreach (var coin in coins)
            if (coin is not null) coin.Save();
        _cat.Save();
        foreach (var crate in crates)
            if (crate is not null) crate.Save();
        foreach (var scorpio in scorpios)
            if (scorpio is not null) scorpio.Save();
        chest.Save();
        grabbingHook.Save();
        takeKey.Save();
        manageButtons.Save();
        foreach (var door in doors)
            door.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
    }
}