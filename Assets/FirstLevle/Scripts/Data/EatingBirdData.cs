using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EatingBirdData : IInitializable<EatingBird>
{
    public static HashSet<string> start = new ();
    public Dictionary<string, float[]> positions;
    public int animatorState;

    public EatingBirdData() 
    {
        positions = new Dictionary<string, float[]>(); 
    } 
    
    public void Initialize(EatingBird eatingBird)
    {
        var value = new float[3];
        var obj = eatingBird.gameObject;
        animatorState = eatingBird.GetComponent<Animator>().GetInteger("state");
        if (positions.TryGetValue(eatingBird.gameObject.name, out value))
        {
            value[0] = obj.transform.position.x; 
            value[1] = obj.transform.position.y; 
            value[2] = obj.transform.position.z; 
        }
        else
        {
            positions.Add(eatingBird.gameObject.name, new []{
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z});
        }
    } 
}
