using System;
using System.Collections.Generic;

[Serializable]
public class CrateData : IInitializable<Crate>
{
    public HashSet<string> start { get; set;} = new();
    public Dictionary<string, float[]> positions;
    
    public CrateData() 
    {
        positions = new Dictionary<string, float[]>(); 
    } 
    
    public void Initialize(Crate crate)
    {
        var value = new float[3];
        var obj = crate.gameObject;
        if (positions.TryGetValue(crate.gameObject.name, out value))
        {
            value[0] = obj.transform.position.x; 
            value[1] = obj.transform.position.y; 
            value[2] = obj.transform.position.z; 
        }
        else
        {
            positions.Add(crate.gameObject.name, new []{
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z});
        }
    } 
}
