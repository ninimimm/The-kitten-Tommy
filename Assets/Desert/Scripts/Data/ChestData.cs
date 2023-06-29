using System;
using System.Collections.Generic;
[Serializable]

public class ChestData : IInitializable<Chest>
{
    public HashSet<string> start { get; set;} = new();
    public int animatorState;
    public bool isOpened;

    public ChestData(){}
    
    public void Initialize(Chest chest)
    {
        isOpened = chest.isOpened;
        animatorState = chest._animator.GetInteger("state");
    } 
}