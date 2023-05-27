using System;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[Serializable]

public class KnifeData : IInitializable<Knife>
{
    public static HashSet<string> start = new ();
    public float attackIntervale;

    public KnifeData(){}
    
    public void Initialize(Knife knife)
    {
        attackIntervale = knife.attackIntervale;
    } 
}