using System;
using System.Collections.Generic;
[Serializable]

public class MainMenuData : IInitializable<MainMenu>
{
    public HashSet<string> start { get; set;} = new();
    public int saveForPlay;

    public MainMenuData(){}
    
    public void Initialize(MainMenu mainMenu)
    {
        saveForPlay = mainMenu.saveForPlay;
    } 
}