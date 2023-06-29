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
    private string saveIndexFilePath = "C:\\Users\\nik_chern\\Desktop\\Остальное\\TheGame\\The-kitten-Tommy\\Assets\\MainMenu\\Scripts\\saveIndex.txt";
    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
        if (File.Exists(saveIndexFilePath))
        {
            using (StreamReader sr = File.OpenText(saveIndexFilePath))
            {
                saveIndex = int.Parse(sr.ReadLine());
            }
        }   
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
        using (StreamWriter sw = File.CreateText(saveIndexFilePath))
        {
            sw.WriteLine(saveIndex);
        }
        Application.Quit();
    }
}