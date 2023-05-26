using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BirdStayData : IInitializable<BirdStay>
{
    public static HashSet<string> start = new ();
    public Dictionary<string, float[]> positions;
    public int animatorState;

    public BirdStayData() 
    {
        positions = new Dictionary<string, float[]>(); 
    } 
    
    public void Initialize(BirdStay birdStay)
    {
        var value = new float[3];
        var obj = birdStay.gameObject;
        animatorState = birdStay.GetComponent<Animator>().GetInteger("state");
        if (positions.TryGetValue(birdStay.gameObject.name, out value))
        {
            value[0] = obj.transform.position.x; 
            value[1] = obj.transform.position.y; 
            value[2] = obj.transform.position.z; 
        }
        else
        {
            positions.Add(birdStay.gameObject.name, new []{
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z});
        }
    } 
}
