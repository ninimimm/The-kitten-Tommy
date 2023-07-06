using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panLimitLeft = -100f;
    public float panLimitRight = 100f;
    public float panLimitUp = 100f;
    public float panLimitDown = -100f;


    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey(KeyCode.Mouse0))
        {
            pos.x += Input.GetAxis("Mouse X") * panSpeed * Time.deltaTime;
            pos.y += Input.GetAxis("Mouse Y") * panSpeed * Time.deltaTime;
        }

        pos.x = Mathf.Clamp(pos.x, panLimitLeft, panLimitRight);
        pos.y = Mathf.Clamp(pos.y, panLimitDown, panLimitUp);
        transform.position = pos;
    }
}