using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button start;
    public static List<bool> isStarts = new ();
    public static int index;
    public static int saveIndex;
    public static  bool isResume;
    public static string saveIndexFilePath = "saveIndex.txt";
    private static string pathFile = "pathFile.txt";

    private void Start()
    {
        for (var i = 0; i < 1000000; i++)
            isStarts.Add(true);
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
        if (File.Exists(pathFile))
        {
            using (StreamReader sw = File.OpenText(pathFile))
            {
                foreach (var line in sw.ReadToEnd().Split("\n"))
                {
                    if (File.Exists(line))
                        File.Delete(line);
                }
            }
        }
        for (var i = 0; i < 1000000; i++)
            isStarts[i] = true;
        index = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PlayGameResume()
    {
        for (var i = 0; i < 1000000; i++)
            isStarts[i] = false;
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
        Debug.Log(saveIndexFilePath);
        Application.Quit();
    }
}