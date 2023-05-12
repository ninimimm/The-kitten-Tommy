using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GrabbingHook : MonoBehaviour
{
    [SerializeField] public float distanse = 10f;
    [SerializeField] public float stopping = 0.2f;
    [SerializeField] private float distanseIn;
    [SerializeField] private AudioClip woosh;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    public bool isHooked;
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
        _audioSource = GetComponent<AudioSource>();
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
                isHooked = true;
                _joint2D.enabled = true;
                _joint2D.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2D.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2D.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position-new Vector3(-0.1f,0.1f,0));
                line.SetPosition(1,_raycast.point + new Vector2(distanseIn,distanseIn));
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            
            if (isHooked && 
                Vector3.Distance(line.GetPosition(1),line.GetPosition(0) + new Vector3(-0.1f,0.1f,0)) - 
                (line.GetPosition(1).y - GetComponent<CatSprite>().transform.position.y) < 0.1
                && !_audioSource.isPlaying && Math.Abs(GetComponent<CatSprite>()._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0,transform.position - new Vector3(-0.1f,0.1f,0));
            line.SetPosition(1, _joint2D.connectedBody.transform.position + (Vector3)_joint2D.connectedAnchor);
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isHooked = false;
            _joint2D.enabled = false;
            line.enabled = false;
        }
    }
}
