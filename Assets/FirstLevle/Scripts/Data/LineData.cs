using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LineData : IInitializable<Line>
{
    public static HashSet<string> start = new ();
    public string material;
    
    public LineData() {}

    public void Initialize(Line line)
    {
        material = line.material;
    }
}