using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class BirdIdleData : IInitializable<BirdIdle>
{
    public static HashSet<string> start = new ();
    public Dictionary<string, float[]> positions;
    public int animatorState;

    public BirdIdleData() 
    {
        positions = new Dictionary<string, float[]>(); 
    } 
    
    public void Initialize(BirdIdle birdIdle)
    {
        var value = new float[3];
        var obj = birdIdle.gameObject;
        animatorState = birdIdle.GetComponent<Animator>().GetInteger("state");
        if (positions.TryGetValue(birdIdle.gameObject.name, out value))
        {
            value[0] = obj.transform.position.x; 
            value[1] = obj.transform.position.y; 
            value[2] = obj.transform.position.z; 
        }
        else
        {
            positions.Add(birdIdle.gameObject.name, new []{
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z});
        }
    } 
}
