using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool pauseGame;
    public GameObject pauseGameMenu;
    public GameObject helpGameMenu;
    [SerializeField] private GameObject[] crates;
    [SerializeField] private GameObject[] stones;
    [SerializeField] private GoToSecondLevle goToSecond;
    [SerializeField] private GoToFirstLevel goToFirst;
    [SerializeField] private GoToSnow goToSnow;
    private Vector3[] _transforms = new Vector3[5];
    private Vector3[] _transformsStones = new Vector3[3];
    private int i;
    
    private void Start()
    {
        if(crates.Length > 0)
            for (var i = 0; i < crates.Length; i++)
                _transforms[i] = crates[i].transform.position;
        if (stones.Length > 0)
            for (var i = 0; i < stones.Length; i++)
                _transformsStones[i] = stones[i].transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "FirstLevleComic" && Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
        helpGameMenu.SetActive(false);
        Time.timeScale = 1f;
        pauseGame = false;
    }
    
    public void Pause()
    {
        pauseGameMenu.SetActive(true);
        Time.timeScale = 0f;
        pauseGame = true;
    }

    public void Help()
    {
        if (i % 2 == 0)
        {
            helpGameMenu.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            helpGameMenu.SetActive(false);
            Time.timeScale = 1f;
        }
        i++;
    }

    public void LoadMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex > 1)
        {
            Debug.Log("save");
            MainMenu.saveIndex = SceneManager.GetActiveScene().buildIndex;
            using (StreamWriter sw = File.CreateText(MainMenu.saveIndexFilePath))
            {
                sw.WriteLine(MainMenu.saveIndex);
            }
        }
            
        if (MainMenu.saveIndex == 2)
            goToSecond.Save();
        else if (MainMenu.saveIndex == 3)
            goToFirst.Save();
        else goToSnow.Save();
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
    public void RespaumStones()
    {
        for (var i = 0; i < stones.Length; i++)
            stones[i].transform.position = _transformsStones[i];
    }
}
