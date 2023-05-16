using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class TextComic : MonoBehaviour
{
    [SerializeField] public Text TextGameObject;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private float waitToRead = 2f;

    private Queue<string> textsToWrite = new Queue<string>();

    void Start()
    {
        textsToWrite.Enqueue("На обширных просторах песчаной пустыни, где дюны подобны рыжим волнам океана, стоял небольшой домик. В нем жила семья кота Томми. Их шерсть была такого же теплого рыжего цвета, как и песок вокруг");
        textsToWrite.Enqueue("Вместе они проводили солнечные дни, охотясь на проворных песчанок и тщательно ухаживая за грядками ароматной мяты");
        textsToWrite.Enqueue("Однажды, в один прекрасный солнечный денек, пока остальные кошки были заняты охотой, маленький Томми отвлекся на игривый листик, пляшущий в ветре. Он весело гнался за ним, забыв обо всем на свете");
        textsToWrite.Enqueue("Листик заводил его все дальше и дальше от дома, пока громкий звук молнии не остановил его");
        textsToWrite.Enqueue("Томми обернулся и увидел надвигающееся торнадо. Воздух был наполнен электричеством, а песок поднимался вверх, образуя огромный вихрь. Перед глазами Томми, этот мощный вихрь подхватил его ничего не подозревающую семью и унес их прочь");
        textsToWrite.Enqueue("Ошеломленный, он смотрел, как они исчезают в песчаной буре");
        textsToWrite.Enqueue("Томми ворвался в дом, где его ждал мудрый дедушка-кот. Следя за бурей из окна, дедушка с грустью в глазах рассказал Томми, что его семью унесло в облачный мир, где живет злобный пес Брут");
        textsToWrite.Enqueue("Он вручил Томми старую карту и объяснил ему, как добраться до этого мира, чтобы спасти свою семью");
        textsToWrite.Enqueue("И котенок Томми собрал все свои силы в кулак и отправился в путешествие, чтобы спасти свою семью");
        StartCoroutine(WriteTexts());
        
    }
    
    IEnumerator WriteTexts()
    {
        while (textsToWrite.Count > 0)
        {
            string textToWrite = textsToWrite.Dequeue();
            yield return StartCoroutine(WriteText(textToWrite));
            yield return StartCoroutine(Clear(textToWrite));
        }
        
    }

    IEnumerator WriteText(string textToWrite)
    {
        foreach (char c in textToWrite)
        {
            TextGameObject.text += c;
            yield return new WaitForSeconds(speed); 
        }
        yield return new WaitForSeconds(waitToRead);
    }

    IEnumerator Clear(string clearText)
    {
        for (var i = clearText.Length; i > -1; i--)
        {
            TextGameObject.text = clearText.Substring(0, i);
            yield return new WaitForSeconds(speed/5);
        }
        yield return new WaitForSeconds(1f);
    }
}
