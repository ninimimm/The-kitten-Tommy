using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class ChestData : IInitializable<Chest>
{
    public HashSet<string> start { get; set;} = new();
    public int animatorState;
    public bool isOpened;
    public bool isEmpty;

    public ChestData(){}
    
    public void Initialize(Chest chest)
    {
        isOpened = chest.isOpened;
        animatorState = chest._animator.GetInteger("state");
        isEmpty = chest.isEmpty;
    } 
}