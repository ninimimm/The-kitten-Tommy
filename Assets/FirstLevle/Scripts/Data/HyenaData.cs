using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class HyenaData : IInitializable<Hyena>
{
    public static HashSet<string> start = new ();
    public float HP;
    public bool polyEnabled;
    public bool capEnabled;
    public int animatorState;
    public float[] position;
    
    public HyenaData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Hyena hyena)
    {
        HP = hyena.HP;
        polyEnabled = hyena.pol.enabled;
        capEnabled = hyena.cap.enabled;
        animatorState = hyena.animator.GetInteger("state");
        position[0] = hyena.transform.position.x;
        position[1] = hyena.transform.position.y;
        position[2] = hyena.transform.position.z;
    } 
}