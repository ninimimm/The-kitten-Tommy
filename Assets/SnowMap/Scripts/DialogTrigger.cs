using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogManager _dialogManager;
    public Dialog dialog;

    public string startTag; // Добавьте это поле для хранения тега начального узла

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            TriggerDialog();
    }


    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }

    public void TriggerDialogFromTag() // Добавьте этот метод для вызова диалога из тега
    {
        FindObjectOfType<DialogManager>().StartDialogFromTag(dialog, startTag);
    }
}