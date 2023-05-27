using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public interface IInitializable<TObject>
{
    void Initialize(TObject obj);
}

public static class SavingSystem <TObject,TData>
    where TData: class, IInitializable<TObject>, new()
    where TObject : MonoBehaviour
{
    private static BinaryFormatter binaryFormatter;
    private static TData data = new ();
    private static string fullPath;
    public static void Save(TObject obj, string path)
    {
        binaryFormatter = new BinaryFormatter();
        fullPath = $"{Application.persistentDataPath}{path}";
        data.Initialize(obj);
        using (var stream = new FileStream(fullPath, FileMode.Create))
            binaryFormatter.Serialize(stream, data);
    }
    
    public static TData Load(string path)
    {
        fullPath = $"{Application.persistentDataPath}{path}";
        if (File.Exists(fullPath))
        {
            using (var stream = new FileStream(fullPath, FileMode.Open))
            {
                if (stream.Length == 0)
                    data = new TData();
                else
                {
                    binaryFormatter = new BinaryFormatter();
                    data = binaryFormatter.Deserialize(stream) as TData;
                }
            }
            return data;
        }
        return new TData();
    }

}