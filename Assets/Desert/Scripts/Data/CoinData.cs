using System;
using System.Collections.Generic;

[Serializable] 
public class CoinData : IInitializable<Coin>
{
    public HashSet<string> start { get; set;} = new();
    public Dictionary<string, float[]> positions;

    public CoinData() 
    {
        positions = new Dictionary<string, float[]>(); 
    } 
 
    public void Initialize(Coin coin)
    {
        var value = new float[3];
        var obj = coin.gameObject;
        if (positions.TryGetValue(coin.gameObject.name, out value))
        {
            value[0] = obj.transform.position.x; 
            value[1] = obj.transform.position.y; 
            value[2] = obj.transform.position.z; 
        }
        else
        {
            positions.Add(coin.gameObject.name, new []{
                obj.transform.position.x,
                obj.transform.position.y,
                obj.transform.position.z});
        }
    } 
}