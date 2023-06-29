using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button start;
    public static List<string> allPaths = new ();
    public static List<HashSet<string>> starts = new ();
    public static int saveIndex;
    public static  bool isResume;
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
    }

    public void PlayGame()
    {
        isResume = false;
        foreach (var path in allPaths)
            if (File.Exists(path))
                File.Delete(path);
        for (var i = 0; i < starts.Count; i++)
            starts[i] = new HashSet<string>();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGameResume()
    {
        isResume = true;
        SceneManager.LoadScene(saveIndex);
    }
    
    
    public void PlayTrainer()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
