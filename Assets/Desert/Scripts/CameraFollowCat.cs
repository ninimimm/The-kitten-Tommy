using UnityEngine;

public class CameraFollowCat : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float smoothSpeed = 0.125f;
    public bool maintainY;
    [SerializeField] public float startY;
    [SerializeField] private Vector2 defaulResolution = new Vector2(1080, 1980);
    [SerializeField] private float leftx;
    [SerializeField] private float rightx;
    [Range(0f, 1f)] public float widthOrHeight;
    public Camera camera;
    private float initialSize;
    private float targetAspect;
    private float value;
    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void Start()
    {
        camera = GetComponent<Camera>();
        initialSize = camera.orthographicSize;
        targetAspect = defaulResolution.x / defaulResolution.y;
    }

    private void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.Tab))
        {
            var constantWidthSize = initialSize * (targetAspect / camera.aspect);
            camera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, widthOrHeight);
            if (target.position.x <= leftx) value = leftx;
            else if (target.position.x >= rightx) value = rightx;
            else value = target.position.x;
            desiredPosition = new Vector3(value + offset.x, maintainY ? transform.position.y+startY : target.position.y + offset.y+startY, transform.position.z);
            smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }
}