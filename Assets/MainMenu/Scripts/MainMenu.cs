using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] private UnityEngine.UI.Button start;
    public static Dictionary<string, int> dictSave = new();
    public static List<bool> isStarts = new ();
    public static int index;
    public static int saveIndex;
    public static  bool isResume;
    public static string saveIndexFilePath;
    private static string pathFile;

    public Dictionary<string, int> dictSaveNonStatic = new();
    public List<bool> isStartsNonStatic = new ();

    private MainMenuData data;
    private bool play;
    private float timer = 2;

    private void Start()
    {
        #if UNITY_STANDALONE_WIN
        Application.targetFrameRate = 240;
        #elif UNITY_ANDROID     
        Application.targetFrameRate = 60;
        #endif
        
        saveIndexFilePath = $"{Application.persistentDataPath}/saveIndex.txt";
        pathFile = $"{Application.persistentDataPath}/pathFile.txt";
        Debug.Log(Application.persistentDataPath);
        if (!File.Exists(pathFile)) File.Create(pathFile);
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(start.gameObject);
        if (File.Exists(saveIndexFilePath))
        {
            using (StreamReader sr = File.OpenText(saveIndexFilePath))
            {
                saveIndex = int.Parse(sr.ReadLine());
            }
        }
        Load();
        if (isStarts.Count == 0)
        {
            for (var i = 0; i < 1000000; i++)
                isStarts.Add(true);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") _canvas.SetActive(true);
        else _canvas.SetActive(false);
    }

    public void Save()
    {
        if (this != null)
        {
            SaveToNonStatic();
            SavingSystem<MainMenu, MainMenuData>.Save(this, $"{gameObject.name}.data");
        }
    }


    public void Load()
    {
        data = SavingSystem<MainMenu, MainMenuData>.Load($"{gameObject.name}.data");
        dictSave = data.dictSave;
        isStarts = data.isStarts;
    }
    
    public void SaveToNonStatic()
    {
        dictSaveNonStatic = new Dictionary<string, int>(dictSave);
        isStartsNonStatic = new List<bool>(isStarts);
    }
    
    public void PlayGame()
    {
        isResume = false;
        if (System.IO.File.Exists(pathFile)) System.IO.File.WriteAllText(pathFile,string.Empty);
        foreach (var path in System.IO.Directory.GetFiles(Application.persistentDataPath))
            if (System.IO.Path.GetExtension(path).ToLower().Contains(".data")) System.IO.File.Delete(path);

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
        dictSaveNonStatic = new Dictionary<string, int>(dictSave);
        isStartsNonStatic = new List<bool>(isStarts);
        Save();
        using (StreamWriter sw = File.CreateText(saveIndexFilePath))
        {
            sw.WriteLine(saveIndex);
        }
        Debug.Log(saveIndexFilePath);
        QuitApplication();
    }
    
    private void QuitApplication()
    {
        #if UNITY_STANDALONE_WIN
        Application.Quit();
        #elif UNITY_ANDROID
        AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer")
        .GetStatic<AndroidJavaObject>("currentActivity");
        activity.Call("moveTaskToBack", true);
        #endif
    }
}