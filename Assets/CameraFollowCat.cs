using UnityEngine;

public class CameraFollowCat : MonoBehaviour
{
    public Transform target;
    public Vector2 offset;
    public float smoothSpeed = 0.125f;
    public bool maintainY = false;

    private void FixedUpdate()
    {
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, maintainY ? transform.position.y : target.position.y + offset.y, transform.position.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}