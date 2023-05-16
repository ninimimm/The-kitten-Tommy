using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class Move : MonoBehaviour
{
    [SerializeField] private float speedImage;
    [SerializeField] [NotNull] private Text textImage;
    [SerializeField] private float diff;
    private bool triger;
    // Start is called before the first frame udate
    void Start()    
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<TextComic>().TextGameObject.text ==
            "Листик заводил его все дальше и дальше от дома, пока громкий звук молнии не остановил его")
            triger = true;
        if (triger)
        {
            transform.position -= new Vector3(speedImage + 0.01f,0,0);
            textImage.transform.position += new Vector3(speedImage + 0.01f, 0, 0);
        }
        else
        {
            transform.position -= new Vector3(speedImage + diff,0,0);
            textImage.transform.position += new Vector3(speedImage + diff, 0, 0);
        }
    }
}
