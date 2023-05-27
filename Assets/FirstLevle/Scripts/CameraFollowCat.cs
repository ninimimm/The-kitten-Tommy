using UnityEngine;

public class CameraFollowCat : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float smoothSpeed = 0.125f;
    public bool maintainY = false;
    [SerializeField] public float startY;
    [SerializeField] private Vector2 defaulResolution = new Vector2(1080, 1980);
    [Range(0f, 1f)] public float widthOrHeight;
    public Camera _camera;
    private float initialSize;
    private float targetAspect;
    private float value;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        initialSize = _camera.orthographicSize;
        targetAspect = defaulResolution.x / defaulResolution.y;
    }

    private void FixedUpdate()
    {
        var constantWidthSize = initialSize * (targetAspect / _camera.aspect);
        _camera.orthographicSize = Mathf.Lerp(constantWidthSize, initialSize, widthOrHeight);
        if (target.position.x <= -20) value = -20f;
        else if (target.position.x >= 48f) value = 48f;
        else value = target.position.x;
        Vector3 desiredPosition = new Vector3(value + offset.x, maintainY ? transform.position.y+startY : target.position.y + offset.y+startY, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}