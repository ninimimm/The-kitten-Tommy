using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pinguin : MonoBehaviour
{
    [SerializeField] private Image gui;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI skip;
    [SerializeField] private GameObject cat;

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, cat.transform.position) < 2)
        {
            gui.enabled = true;
            text1.enabled = true;
            text2.enabled = true;
            skip.enabled = true;
        }
        else
        {
            gui.enabled = false;
            text1.enabled = false;
            text2.enabled = false;
            skip.enabled = false;
        }
    }
}
