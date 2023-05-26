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
    public static List<GameObject> coins = new();
    [SerializeField] private GameObject[] crates;
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
        {
            if (coin != null) coin.GetComponent<Coin>().Save();
        }
            
        _cat.GetComponent<CatSprite>().Save();
        foreach (var crate in crates)
            crate.GetComponent<Crate>().Save();
    }
}
