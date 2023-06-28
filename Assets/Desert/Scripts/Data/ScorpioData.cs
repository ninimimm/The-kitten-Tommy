using System;
using System.Collections.Generic;

[Serializable]
public class ScorpioData : IInitializable<Scorpio>
{
    public static HashSet<string> start = new ();
    public float HP;
    public bool polyEnabled;
    public bool boxEnabled;
    public int animatorState;
    public float[] position;
    
    public ScorpioData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Scorpio scorpio)
    {
        var obj = scorpio.gameObject;
        HP = scorpio.HP;
        polyEnabled = scorpio.polygonCollider.enabled;  
        boxEnabled = scorpio.boxCollider.enabled;
        animatorState = scorpio.animator.GetInteger("stateScorpio");
        position[0] = scorpio.transform.position.x;
        position[1] = scorpio.transform.position.y;
        position[2] = scorpio.transform.position.z;
    } 
}