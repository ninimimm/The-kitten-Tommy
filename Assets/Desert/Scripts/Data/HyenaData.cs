using System;
using System.Collections.Generic;

[Serializable]
public class HyenaData : IInitializable<Hyena>
{
    public HashSet<string> start { get; set;} = new();
    public float HP;
    public bool cap1Enabled;
    public bool cap2Enabled;
    public int animatorState;
    public float[] position;
    
    public HyenaData()
    {
        position = new float[3];
    } 
    
    public void Initialize(Hyena hyena)
    {
        HP = hyena.HP;
        cap1Enabled = hyena.cap1.enabled;
        cap2Enabled = hyena.cap2.enabled;
        animatorState = hyena.animator.GetInteger("state");
        position[0] = hyena.transform.position.x;
        position[1] = hyena.transform.position.y;
        position[2] = hyena.transform.position.z;
    } 
}