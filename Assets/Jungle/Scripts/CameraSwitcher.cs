using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer middleground;
    [SerializeField] private GameObject cat;
    // Изначальное положение камеры
    private Vector3 initialPosition;
    private Vector3 initialRotation;
    private Vector3 initialScale;

    // Целевые значения для перемещения и изменения размера камеры
    public Vector3 targetPosition = new Vector3(37.8f, 6.03f, -20f);
    public float targetSize = 6f;

    private Camera cameraComponent;
    private bool camera2;
    private float timer;

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
        if (timer > 0 && !camera2) timer -= Time.deltaTime;
        if (timer < 0)
        {
            background.enabled = false;
            middleground.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (cat.transform.position.y > 13)
            {
                timer = 0.1f;
                background.enabled = true;
                middleground.enabled = true;
                camera2 = !camera2;
            }
            else camera2 = false;
        }
        if (camera2)
        {
            transform.position = targetPosition;
            cameraComponent.orthographicSize = targetSize;
        }
    }
}