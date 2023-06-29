using System;
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
    public static bool isResume;
    public int saveForPlay;
    public MainMenuData data;
    private void Start()
    {
        Load();
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
    }

    public void Save()
    {
        saveForPlay = saveIndex;
        SavingSystem<MainMenu,MainMenuData>.Save(this, $"{gameObject.name}.data");
    }


    public void Load()
    {
        data = SavingSystem<MainMenu, MainMenuData>.Load($"{gameObject.name}.data");
        saveForPlay = data.saveForPlay;
        saveIndex = saveForPlay;
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
        Save();
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
