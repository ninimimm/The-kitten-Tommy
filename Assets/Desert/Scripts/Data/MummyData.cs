using System;
using System.Collections.Generic;

[Serializable]
public class MummyData : IInitializable<Mummy>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public bool polyEnabled;
    public bool capEnabled;
    public int animatorState;
    public float[] position;
    
    public MummyData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Mummy mummy)
    {
        HP = mummy.HP;
        polyEnabled = mummy.pol.enabled;
        capEnabled = mummy.cap.enabled;
        animatorState = mummy.animator.GetInteger("state");
        position[0] = mummy.transform.position.x;
        position[1] = mummy.transform.position.y;
        position[2] = mummy.transform.position.z;
    } 
}