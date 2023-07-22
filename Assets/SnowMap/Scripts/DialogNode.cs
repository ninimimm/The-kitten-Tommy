using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode
{
    public string tag; // тег узла
    [TextArea(3, 10)] 
    public string text; // текст вопроса или ответа
    [SerializeField] public AudioClip audioClip;
    public List<DialogNode> responses; // список возможных ответов
    public string jumpTag; // тег узла для перехода

    public DialogNode(string text)
    {
        this.text = text;
        responses = new List<DialogNode>();
    }
}
