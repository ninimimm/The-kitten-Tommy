using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    private Queue<DialogNode> sentences;
    private Queue<string> responsesQueue;
    private List<DialogNode> currentResponses;
    public TextMeshProUGUI dialogText1;
    public TextMeshProUGUI dialogText2;
    public bool dialogIsStart;
    public float typingSpeed = 0.02f;  // Скорость отображения текста. Вы можете настроить это значение.
    private bool isTyping = false;
    private bool stopTyping = false; 
    private Dialog currentDialog;

    public bool IsTyping()
    {
        return isTyping;
    }

    void Start()
    {
        sentences = new Queue<DialogNode>();
        responsesQueue = new Queue<string>();
        currentResponses = new List<DialogNode>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            stopTyping = true;
        }
        for (int i = 1; i <= currentResponses.Count; i++)
        {
            if (Input.GetKeyDown(i.ToString()) && !isTyping)
            {
                var selectedResponse = currentResponses[i-1];
                OnResponseSelected(currentDialog, selectedResponse, selectedResponse.jumpTag);
                dialogText2.text = "";
                break;
            }
        }
    }



    public void StartDialog(Dialog dialog)
    {
        currentDialog = dialog;
        dialogIsStart = true;
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
        StartCoroutine(TypeSentence(current.text, dialogText1));

        currentResponses.Clear();
        responsesQueue.Clear();
        int responseNumber = 1;
        foreach (var response in current.responses)
        {
            currentResponses.Add(response);
            responsesQueue.Enqueue($"{responseNumber}. {response.text}\n");
            responseNumber++;
        }
    }

    IEnumerator TypeSentence(string sentence, TextMeshProUGUI textComponent)
    {
        stopTyping = false;
        isTyping = true;
        textComponent.text = "";
        foreach (char letter in sentence)
        {
            textComponent.text += letter;
            if (stopTyping)
            {
                textComponent.text = sentence;
                break;
            }
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        if (responsesQueue.Count > 0)
        {
            dialogText2.text = "";
            StartCoroutine(TypeResponses(dialogText2));
        }
    }

    IEnumerator TypeResponses(TextMeshProUGUI textComponent)
    {
        stopTyping = false;
        isTyping = true;
        while (responsesQueue.Count > 0)
        {
            string response = responsesQueue.Dequeue();
            int letterCount = 0; // Добавьте счетчик символов
            foreach (char letter in response)
            {
                textComponent.text += letter;
                letterCount++; // Увеличивайте счетчик на каждой итерации
                if (stopTyping) 
                {
                    // Печатайте оставшуюся часть ответа, отсекая уже напечатанную часть
                    textComponent.text += response.Substring(letterCount);
                    while (responsesQueue.Count > 0)
                    {
                        textComponent.text += responsesQueue.Dequeue();
                    }
                    break;
                }
                yield return new WaitForSeconds(typingSpeed);
            }
        }
        isTyping = false;
    }


    public void OnResponseSelected(Dialog dialog, DialogNode selectedResponse, string tag)
    {
        if (!string.IsNullOrEmpty(tag))
        {
            StartDialogFromTag(dialog, tag);
            return;
        }

        // Если тег не указан, продолжаем с текущими ответами
        foreach (var node in selectedResponse.responses)
            sentences.Enqueue(node);

        DisplayNextSentense();
    }

    
    public void StartDialogFromTag(Dialog dialog, string startTag)
    {
        currentDialog = dialog;
        DialogNode startingNode = FindNodeByTag(dialog.root, startTag);
        if (startingNode == null)
        {
            Debug.LogError($"No node found with tag {startTag}");
            return;
        }
    
        dialogIsStart = true;
        sentences.Clear();
        sentences.Enqueue(startingNode);
        DisplayNextSentense();
    }

    
    private DialogNode FindNodeByTag(DialogNode node, string tag)
    {
        if (node.tag == tag)
            return node;
        
        foreach (DialogNode child in node.responses)
        {
            DialogNode foundNode = FindNodeByTag(child, tag);
            if (foundNode != null)
                return foundNode;
        }

        return null;
    }
    

    void EndDialog()
    {
        dialogIsStart = false;
    }
}
