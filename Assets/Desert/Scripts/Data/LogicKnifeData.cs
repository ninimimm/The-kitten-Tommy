using System;
using System.Collections.Generic;

[Serializable]

public class LogicKnifeData : IInitializable<logicKnife>
{
    public HashSet<string> start { get; set;} = new();
    public float damage;

    public LogicKnifeData(){}
    
    public void Initialize(logicKnife knife)
    {
        damage = knife.damage;
    } 
}