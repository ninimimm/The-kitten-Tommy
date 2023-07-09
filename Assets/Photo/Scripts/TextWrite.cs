using System.Collections;
using TMPro;
using UnityEngine;

public class TextWrite : MonoBehaviour
{
    public TextMeshProUGUI textField;
    public Vector3 vector = new (-6,7,0);
    public string[] textPackages;
    public float[] delays;
    public float typingSpeed = 0.05f;
    public bool isE;

    private Coroutine typingCoroutine;
    private int currentPackageIndex = 0;

    private void Start()
    {
        StartTyping();
    }

    public void StartTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeTextPackages());
    }

    private IEnumerator TypeTextPackages()
    {
        for (int i = 0; i < textPackages.Length; i++)
        {
            string currentText = textPackages[i];
            yield return StartCoroutine(TypeText(currentText));

            if (i < delays.Length)
                yield return new WaitForSeconds(delays[i]);

            // Стираем предыдущий текст перед печатью нового
            textField.text = "";
        }
    }

    private IEnumerator TypeText(string text)
    {
        foreach (char c in text)
        {
            textField.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}