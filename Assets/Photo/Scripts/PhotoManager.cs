using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PhotoManager : MonoBehaviour
{
    [SerializeField] private Transform cat;
    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private Image im;
    [SerializeField] private Sprite newSprite;
    [SerializeField] private TextWrite textWrite;
    private SpriteRenderer image;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialScale;

    private void Start()
    {
        image = GetComponent<SpriteRenderer>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (Vector3.Distance(
                new Vector3(cat.transform.position.x, cat.transform.position.y - 0.5f, cat.transform.position.z), transform.position) < 0.5f
                && Input.GetKeyDown(KeyCode.E) && image.enabled && !textWrite.isE)
        {
            StartCoroutine(Transition());
        }
    }

    private IEnumerator Transition()
    {
        textWrite.isE = true;
        image.sortingLayerName = "GUI";
        // Плавное перемещение в указанные координаты
        float duration = 2f;
        Vector3 targetPosition = new Vector3(0.8f, 3.4f, 0f);
        Quaternion targetRotation = Quaternion.identity;
        Vector3 targetScale = transform.localScale * 11f;

        Vector3 startPosition = transform.position;
        Quaternion startRotation = transform.rotation;
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = targetScale;

        // Появление другого изображения и плавное увеличение
        yield return new WaitForSeconds(0.5f); // Задержка перед показом нового изображения

        duration = 3f;
        im.enabled = true;
        Color targetColor = im.color;
        targetColor.a = 1f; // Целевое значение альфа-канала (полная непрозрачность)
        Color startColor = im.color;
        startColor.a = 0f;
        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            im.color = Color.Lerp(startColor, targetColor, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        im.color = targetColor;
        
        
        
        textWrite.isE = false;
        duration = 2f;
        targetPosition = textWrite.vector;
        textWrite.vector.x += 0.35f;
        image.sprite = newSprite;
        im.enabled = false;
        targetScale = transform.localScale / 11f;

        startPosition = transform.position;
        startScale = transform.localScale;

        elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPosition;
        transform.localScale = targetScale;
        _textMeshPro.text = $"{int.Parse(_textMeshPro.text.Split("/")[0]) + 1}/{_textMeshPro.text.Split("/")[1]}";
        StopAllCoroutines();
    }
}