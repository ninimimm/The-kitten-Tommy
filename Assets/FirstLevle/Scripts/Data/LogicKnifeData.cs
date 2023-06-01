using System;
using System.Collections.Generic;

[Serializable]

public class LogicKnifeData : IInitializable<logicKnife>
{
    public static HashSet<string> start = new ();
    public float damage;

    public LogicKnifeData(){}
    
    public void Initialize(logicKnife knife)
    {
        damage = knife.damage;
    } 
}