using System;
using UnityEngine;

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
    public DistanceJoint2D _joint2D;

    private Vector3 target;

    private RaycastHit2D _raycast;
    
    private CatSprite _catSprite;
    private Camera _mainCamera;


    void Start()
    {
        _joint2D = GetComponent<DistanceJoint2D>();
        _joint2D.enabled = false;
        line.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _catSprite = GetComponent<CatSprite>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_joint2D.distance > 3f)
        {
            _joint2D.distance -= stopping; 
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            _raycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, mask);
            if (_raycast.collider is not null)
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
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (isHooked && 
                Vector3.Distance(line.GetPosition(1), line.GetPosition(0) + new Vector3(-0.1f, 0.1f, 0)) - 
                (line.GetPosition(1).y - _catSprite.transform.position.y) < 0.1
                && !_audioSource.isPlaying && Math.Abs(_catSprite._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0, transform.position - new Vector3(-0.1f, 0.1f, 0));
            if (_joint2D.connectedBody is not null)
            {
                line.SetPosition(1, _joint2D.connectedBody.transform.position + (Vector3)_joint2D.connectedAnchor);
            }
        }

        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isHooked = false;
            _joint2D.enabled = false;
            line.enabled = false;
        }
    }
}
