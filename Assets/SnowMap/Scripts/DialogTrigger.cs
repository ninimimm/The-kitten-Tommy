using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    [SerializeField] private DialogManager _dialogManager;
    public Dialog dialog;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            if (_dialogManager.dialogIsStart)
                _dialogManager.DisplayNextSentense();
            else TriggerDialog();

    }

    public void TriggerDialog()
    {
        FindObjectOfType<DialogManager>().StartDialog(dialog);
    }
}
