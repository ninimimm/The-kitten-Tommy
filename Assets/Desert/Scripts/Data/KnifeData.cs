using System;
using System.Collections.Generic;

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