using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // Изначальное положение камеры
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 initialScale;

    // Целевые значения для перемещения и изменения размера камеры
    public Vector3 targetPosition = new Vector3(37.8f, 6.03f, -20f);
    public float targetSize = 6f;

    private Camera cameraComponent;

    private void Start()
    {
        // Получаем компонент Camera у объекта камеры
        cameraComponent = GetComponent<Camera>();

        // Сохраняем изначальные значения положения, вращения и размера камеры
        initialPosition = transform.position;
        initialRotation = transform.eulerAngles;
        initialScale = transform.localScale;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            transform.position = targetPosition;
            cameraComponent.orthographicSize = targetSize;
        }
    }
}