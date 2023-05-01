using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbingHook : MonoBehaviour
{
    [SerializeField] public float distanse = 10f;
    [SerializeField] public float stopping = 0.2f;
    public LineRenderer line;
    public LayerMask mask;
    private DistanceJoint2D _joint2D;

    private Vector3 target;

    private RaycastHit2D _raycast;
    // Start is called before the first frame update
    void Start()
    {
        _joint2D = GetComponent<DistanceJoint2D>();
        _joint2D.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_joint2D.distance > 3f)
        {
            _joint2D.distance -= stopping;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            _raycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, mask);
            if (_raycast.collider != null)
            {
                _joint2D.enabled = true;
                _joint2D.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2D.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2D.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position);
                line.SetPosition(1,_raycast.point);
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            line.SetPosition(0,transform.position);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _joint2D.enabled = false;
            line.enabled = false;
        }
    }
}
