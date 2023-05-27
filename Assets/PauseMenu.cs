using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool pauseGame;
    public GameObject pauseGameMenu;
    [SerializeField] private GameObject[] crates;
    private Vector3[] _transforms = new Vector3[5];
    
    private void Start()
    {
        for (var i = 0; i < crates.Length; i++)
            _transforms[i] = crates[i].transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "FirstLevleComic" && Input.GetKeyDown(KeyCode.Escape))
            ToLoadMenu();   
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseGame)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseGameMenu.SetActive(false);
        Time.timeScale = 1f;
        pauseGame = false;
    }
    
    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    public void ToLoadMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("MainMenu");
        }
    }
    public void RespaumCrates()
    {
        for (var i = 0; i < crates.Length; i++)
            crates[i].transform.position = _transforms[i];
    }
}
