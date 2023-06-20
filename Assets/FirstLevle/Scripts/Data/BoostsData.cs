using System;
using System.Collections.Generic;

[Serializable]
public class BoostsData : IInitializable<Boosts>
{
    public static HashSet<string> start = new ();
    public int energyCount;
    public int fishCount;
    public int waterCount;
    public float timeToRun;
    public float timeToJump;

    public BoostsData()
    {} 
    
    public void Initialize(Boosts boosts)
    {
        energyCount = boosts.energyCount;
        fishCount = boosts.fishCount;
        waterCount = boosts.waterCount;
        timeToJump = boosts.timeToJump;
        timeToRun = boosts.timeToRun;
    } 
}