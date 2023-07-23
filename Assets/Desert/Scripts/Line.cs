using UnityEngine;
using UnityEngine.SceneManagement;

public class Line : MonoBehaviour
{
    [SerializeField] private LineRenderer line;
    [SerializeField] private Material basedHook;
    [SerializeField] private Material goldHook;
    [SerializeField] private Material coldHook;

    private LineData data;
    public string material;

    private int index = -1;
    // Start is called before the first frame update
    void Start()
    {
        data = SavingSystem<Line, LineData>.Load($"{gameObject.name}.data");
        if (SceneManager.GetActiveScene().name == "FirstLevle")
            material = "based";
        if (!MainMenu.dictSave.ContainsKey(gameObject.name))
        {
            MainMenu.dictSave.Add(gameObject.name,MainMenu.index);
            MainMenu.index ++;
        }
        if (MainMenu.isStarts[MainMenu.dictSave[gameObject.name]])
        {
            Save();
            MainMenu.isStarts[MainMenu.dictSave[gameObject.name]] = false;
        }
        Load();
    }
    public void Save()
    {
        if (this != null) 
            SavingSystem<Line,LineData>.Save(this, $"{gameObject.name}.data");
    }
    
    public void Load()
    {
        data = SavingSystem<Line, LineData>.Load($"{gameObject.name}.data");
        material = data.material;
        if (material == "based")
            line.material = basedHook;
        else if (material == "gold")
            line.material = goldHook;
        else if (material == "cold")
            line.material = coldHook;
    }
}
