using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pinguin : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textE;
    [SerializeField] private SpriteRenderer spriteE;
    [SerializeField] private Image gui;
    [SerializeField] private TextMeshProUGUI text1;
    [SerializeField] private TextMeshProUGUI text2;
    [SerializeField] private TextMeshProUGUI skip;
    [SerializeField] private GameObject cat;
    private bool dialog;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            dialog = true;
        if (Vector3.Distance(transform.position, cat.transform.position) < 2)
        {
            textE.enabled = true;
            spriteE.enabled = true;
        }
        else
        {
            dialog = false;
            textE.enabled = false;
            spriteE.enabled = false;
        }
        if (Vector3.Distance(transform.position, cat.transform.position) < 2 && dialog)
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
