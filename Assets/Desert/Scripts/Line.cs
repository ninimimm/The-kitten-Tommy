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
    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "FirstLevle")
            material = "based";
        if (!LineData.start.Contains(gameObject.name))
        {
            Save();
            LineData.start.Add(gameObject.name);
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
