using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        CutsceneManager.Instance.StartCutscene("CutScene_1");
    }
}
