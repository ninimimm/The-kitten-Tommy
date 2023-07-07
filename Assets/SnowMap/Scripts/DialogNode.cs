using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogNode
{
    public string speaker;
    [TextArea(3, 10)] 
    public string text; // текст вопроса или ответа
    public List<DialogNode> responses; // список возможных ответов

    public DialogNode(string text, string speaker)
    {
        this.speaker = speaker;
        this.text = text;
        this.responses = new List<DialogNode>();
    }
}