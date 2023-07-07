using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<DialogNode> sentences;
    private List<DialogNode> currentResponses;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogText;
    public bool dialogIsStart;

    void Start()
    {
        sentences = new Queue<DialogNode>();
        currentResponses = new List<DialogNode>();
    }
    
    void Update()
    {
        for (int i = 1; i <= currentResponses.Count; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                OnResponseSelected(currentResponses[i-1]);
                break;
            }
        }
    }


    public void StartDialog(Dialog dialog)
    {
        dialogIsStart = true;
        nameText.text = dialog.root.speaker;
        sentences.Clear();
        sentences.Enqueue(dialog.root);
        DisplayNextSentense();
    }

    public void DisplayNextSentense()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        var current = sentences.Dequeue();
        dialogText.text = current.text;
        nameText.text = current.speaker;

        currentResponses.Clear();
        int responseNumber = 1;
        foreach (var response in current.responses)
        {
            currentResponses.Add(response);
            dialogText.text += "\n" + responseNumber + ". " + response.text;
            responseNumber++;
        }
    }

    public void OnResponseSelected(DialogNode selectedResponse)
    {
        // если игрок выбрал ответ, добавляем дочерние узлы этого ответа в очередь
        foreach (var node in selectedResponse.responses)
            sentences.Enqueue(node);
        // отображаем следующую фразу
        DisplayNextSentense();
    }

    
    void EndDialog()
    {
        
    }
}
