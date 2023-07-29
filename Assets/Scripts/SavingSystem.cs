using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

public interface IInitializable<TObject>
{
    HashSet<string> start { get; set; }
    void Initialize(TObject obj);
}

public static class SavingSystem <TObject,TData>
    where TData: class, IInitializable<TObject>, new()
    where TObject : MonoBehaviour
{
    private static BinaryFormatter binaryFormatter;
    private static TData data = new ();
    private static string fullPath;
    private static string pathFile = $"{Application.persistentDataPath}/pathFile.txt";
    public static string aplicationPath = Application.persistentDataPath + "/TheKittenTommy";
    public static void Save(TObject obj, string path)
    {
        binaryFormatter = new BinaryFormatter();
        fullPath = $"{aplicationPath}{path}";
        data.Initialize(obj);
        if (!File.ReadAllText(pathFile).Contains(fullPath));
        using (StreamWriter sw = File.AppendText(pathFile))
            sw.WriteLine(fullPath);
        using (var stream = new FileStream(fullPath, FileMode.Create))
            binaryFormatter.Serialize(stream, data);
    }
    
    public static TData Load(string path)
    {
        fullPath = $"{aplicationPath}{path}";
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