using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class MainMenuData : IInitializable<MainMenu>
{
    public HashSet<string> start { get; set;} = new();
    public Dictionary<string,int> dictSave;
    public List<bool> isStarts;

    public MainMenuData()
    {
        dictSave = new();
        isStarts = new();
    } 
    
    public void Initialize(MainMenu mainMenu)
    {
        dictSave = mainMenu.dictSaveNonStatic;
        isStarts = mainMenu.isStartsNonStatic;
    }
}