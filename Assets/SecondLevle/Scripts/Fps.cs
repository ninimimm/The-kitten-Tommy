using UnityEngine;
using UnityEngine.UI;

public class Fps : MonoBehaviour
{
    public float fps;

    public Text fpsText;

    // Update is called once per frame
    void Update()
    {
        fps = 1 / Time.deltaTime;
        fpsText.text = ((int)fps).ToString();
    }
}
