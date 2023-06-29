using System;
using System.Collections.Generic;

[Serializable]
public class LineData : IInitializable<Line>
{
    public HashSet<string> start { get; set;} = new();
    public string material;
    
    public LineData() {}

    public void Initialize(Line line)
    {
        material = line.material;
    }
}