using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToThirdLevel : MonoBehaviour
{
    [SerializeField] private CatSprite _cat;
    [SerializeField] private Vector3 movingVector;
    [SerializeField] private float speed;
    [SerializeField] private LayerMask balloonLayer;
    [SerializeField] private Scorpio[] scorpios;
    [SerializeField] private Chest chest;
    [SerializeField] private TakeKey takeKey;
    [SerializeField] private Line grabbingHook;
    [SerializeField] private Knife knife;
    [SerializeField] private logicKnife logicKnife;
    [SerializeField] private Boosts boosts;
    [SerializeField] private Door[] doors;
    [SerializeField] private Hyena snowWolf;
    [SerializeField] private Monster[] monsters;
    [SerializeField] private People[] people;
    [SerializeField] private ManageButtons manageButtons;
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
            Physics2D.OverlapCircle(_catSprite.groundCheck2.position, _catSprite.groundCheckRadius, balloonLayer)) &&
            Vector3.Distance(_catSprite.transform.position,transform.position) < 5)
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
    public void Save()
    {
        foreach (var coin in coins) coin.Save();
        _cat.Save();
        foreach (var crate in crates) crate.Save();
        foreach (var scorpio in scorpios) scorpio.Save();
        chest.Save();
        grabbingHook.Save();
        knife.Save();
        logicKnife.Save();
        boosts.Save();
        takeKey.Save();
        foreach (var door in doors) door.Save();
        snowWolf.Save();
        foreach (var monster in monsters) monster.Save();
        foreach (var guy in people) guy.Save();
        manageButtons.Save();
    }
}
