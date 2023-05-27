using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToFirstLevel : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
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
        if (Physics2D.OverlapCircleAll(_catSprite.groundCheck.position, _catSprite.groundCheckRadius, balloonLayer).Length > 0)
        {
            transform.position += movingVector * speed * Time.deltaTime;
            _cat.transform.position += movingVector * speed * Time.deltaTime;
        }

        if (transform.position.y > 10)
        {
            Save();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
            
    }
    private void Save()
    {
        foreach (var coin in coins)
            if (coin != null) coin.GetComponent<Coin>().Save();
        _cat.GetComponent<CatSprite>().Save();
        foreach (var crate in crates)
            if (crate != null) crate.GetComponent<Crate>().Save();
        foreach (var scorpio in scorpios)
            scorpio.Save();
        chest.Save();
        grabbingHook.Save();
        takeKey.Save();
        manageButtons.Save();
        foreach (var door in doors)
            door.Save();
        knife.Save();
        logicKnife.Save();
    }
}