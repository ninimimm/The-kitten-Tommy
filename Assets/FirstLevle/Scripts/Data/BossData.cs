using System;
using System.Collections.Generic;

[Serializable]
public class BossData : IInitializable<SandBoss>
{
    public static HashSet<string> start = new ();
    public float HP;
    public bool polyEnabled;
    public bool capEnabled;
    public int animatorState;
    public float[] position;
    
    public BossData()
    {
        position = new float[3];
    } 
    
    public void Initialize(SandBoss boss)
    {
        HP = boss.HP;
        polyEnabled = boss.pol.enabled;
        capEnabled = boss.cap.enabled;
        animatorState = boss.animator.GetInteger("state");
        position[0] = boss.transform.position.x;
        position[1] = boss.transform.position.y;
        position[2] = boss.transform.position.z;
    } 
}