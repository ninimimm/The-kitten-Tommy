using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TextWrite : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNumber;
    public TextMeshProUGUI textField;
    public Vector3 vector = new Vector3(-6, 7, 0);
    [TextArea(3, 10)]
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

    private void Update()
    {
        if (textNumber.text.Split("/")[0] == "9")
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

            textField.text = ""; // Стираем предыдущий текст перед печатью нового
        }
    }

    private IEnumerator TypeText(string text)
    {
        bool waitForSeconds = false;
        float waitDuration = 0f;

        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (c == '{' && i + 2 < text.Length && text[i + 2] == '}')
            {
                char digitChar = text[i + 1];

                if (char.IsDigit(digitChar))
                {
                    waitDuration = float.Parse(digitChar.ToString());
                    waitForSeconds = true;
                    i += 2; // Skip the "{}" block
                    continue;
                }
            }

            textField.text += c;

            if (waitForSeconds)
            {
                yield return new WaitForSeconds(waitDuration);
                waitForSeconds = false;
            }
            else
            {
                yield return new WaitForSeconds(typingSpeed);
            }
        }
    }
}