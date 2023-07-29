using System;
using UnityEngine;

public class GrabbingHook : MonoBehaviour
{
    [SerializeField] public float distanse = 10f;
    [SerializeField] private float distanseIn;
    [SerializeField] private AudioClip woosh;
    [SerializeField] private float forge;
    [SerializeField] private WriteText writeText;
    [SerializeField] private Joystick joystick;
    [Range(0, 1f)] public float volume;
    private AudioSource _audioSource;
    public bool isHookedStatic;
    public bool isHookedDynamic;
    public LineRenderer line;
    public LayerMask maskStatic;
    public LayerMask maskDynamic;
    public SpringJoint2D _joint2DStatic;
    public DistanceJoint2D _joint2DDynamic;
    

    private Vector3 target;

    private RaycastHit2D _raycast;
    
    private CatSprite _catSprite;
    private Camera _mainCamera;
    private float hookLength;
    private bool _joystickUsed;
    private bool _hookActivated;
    private bool _joystickUsedStatic;
    private bool _hookActivatedStatic;
    private Vector2 _directionJoystickStatic;
    private Vector2 _directionJoystick;
    


    void Start()
    {
        _joint2DStatic = GetComponent<SpringJoint2D>();
        _joint2DStatic.enabled = false;
        _joint2DDynamic = GetComponent<DistanceJoint2D>();
        _joint2DDynamic.enabled = false;
        line.enabled = false;
        _audioSource = GetComponent<AudioSource>();
        _catSprite = GetComponent<CatSprite>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHookedStatic)
            CrateCoinHook();
        if (!isHookedDynamic)
            GroundHook();
    }

    private void FixedUpdate()
    {
        #if UNITY_STANDALONE_WIN
        if (Input.GetKey(KeyCode.Mouse1) && _joint2DStatic.connectedBody is not null && isHookedStatic)
        {
            Vector2 currentDirection = _raycast.point - new Vector2(transform.position.x, transform.position.y);
            float angle = Vector2.Angle(Vector2.up, currentDirection);
            if (angle < 110 && !_catSprite.isGround)
            {
                if (Input.GetKey(KeyCode.A))
                    _catSprite._rb.AddForce(new Vector2(-forge, 0));
                else if (Input.GetKey(KeyCode.D))
                    _catSprite._rb.AddForce(new Vector2(forge, 0));
            }
            if (!_catSprite.isGround && angle > 50 && _catSprite._rb.velocity.y > 0) _catSprite._rb.AddForce(new Vector2(0, -2));
            if (!_catSprite.isGround && angle > 90 && _catSprite._rb.velocity.y > 0) _catSprite._rb.AddForce(new Vector2(0, -4));
        }
        #elif  UNITY_ANDROID
        if (_hookActivatedStatic && _joint2DStatic.connectedBody is not null && isHookedStatic)
        {
            Vector2 currentDirection = _raycast.point - new Vector2(transform.position.x, transform.position.y);
            float angle = Vector2.Angle(Vector2.up, currentDirection);
            if (angle < 110 && !_catSprite.isGround)
            {
                if (_catSprite.joystick.Horizontal < 0)
                    _catSprite._rb.AddForce(new Vector2(-forge, 0));
                else if (_catSprite.joystick.Horizontal > 0)
                    _catSprite._rb.AddForce(new Vector2(forge, 0));
            }
            if (!_catSprite.isGround && angle > 50 && _catSprite._rb.velocity.y > 0) _catSprite._rb.AddForce(new Vector2(0, -2));
            if (!_catSprite.isGround && angle > 90 && _catSprite._rb.velocity.y > 0) _catSprite._rb.AddForce(new Vector2(0, -4));
        }
        #endif
    }

    private void CrateCoinHook()
    {
        #if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            _raycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, maskDynamic);
            var difRaycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, maskStatic);
            if (_raycast.collider is not null &&
                Vector2.Distance(_raycast.point, transform.position)<Vector2.Distance(difRaycast.point, transform.position))
            {
                if (writeText != null && _raycast.collider.gameObject.layer == 12 && writeText.firstTime2)
                {
                    writeText.LKM2.enabled = true;
                    writeText.PKM2.enabled = false;
                    writeText.firstTime2 = false;
                }
                isHookedDynamic = true;
                _joint2DDynamic.enabled = true;
                _joint2DDynamic.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2DDynamic.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2DDynamic.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position - new Vector3(0, 0.55f, 0));
                line.SetPosition(1,_raycast.point + new Vector2(distanseIn,distanseIn));
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (isHookedDynamic && 
                Vector3.Distance(line.GetPosition(1), line.GetPosition(0) + new Vector3(-0.1f, 0.1f, 0)) - 
                (line.GetPosition(1).y - _catSprite.transform.position.y) < 0.47
                && !_audioSource.isPlaying && Math.Abs(_catSprite._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0, transform.position - new Vector3(0, 0.55f, 0));
            if (_joint2DDynamic.connectedBody is not null)
                line.SetPosition(1, _joint2DDynamic.connectedBody.transform.position + (Vector3)_joint2DDynamic.connectedAnchor);
        }
        if (Input.GetKeyUp(KeyCode.Mouse1) || (_joint2DDynamic.connectedBody is not null && !_joint2DDynamic.connectedBody.gameObject.GetComponent<SpriteRenderer>().enabled))
        {
            isHookedDynamic = false;
            _joint2DDynamic.enabled = false;
            line.enabled = false;
        }
        if (isHookedDynamic && _joint2DDynamic.enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            _joint2DDynamic.distance -= scroll;
        }
        #elif UNITY_ANDROID
        if (joystick.Horizontal != 0 || joystick.Vertical != 0) _joystickUsed = true;
        if (Math.Abs(joystick.Horizontal) > 0.6f || Math.Abs(joystick.Vertical) > 0.6f)
            _directionJoystick = joystick.Direction;
        if (joystick.Horizontal == 0 && joystick.Vertical == 0 && _joystickUsed)
        {
            _joystickUsed = false;
            _raycast = Physics2D.Raycast(transform.position, transform.position + 
                                                             new Vector3(_directionJoystick.x,
                                                                 _directionJoystick.y,
                                                                 0)*100, distanse, maskDynamic);
            var difRaycast = Physics2D.Raycast(transform.position, transform.position + 
                                                                   new Vector3(_directionJoystick.x,
                                                                       _directionJoystick.y,
                                                                       0)*100, distanse, maskStatic);
            if (_raycast.collider is not null &&
                Vector2.Distance(_raycast.point, transform.position)<Vector2.Distance(difRaycast.point, transform.position))
            {
                _hookActivated = true;
                if (writeText != null && _raycast.collider.gameObject.layer == 12 && writeText.firstTime2)
                {
                    writeText.LKM2.enabled = true;
                    writeText.PKM2.enabled = false;
                    writeText.firstTime2 = false;
                }
                isHookedDynamic = true;
                _joint2DDynamic.enabled = true;
                _joint2DDynamic.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2DDynamic.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2DDynamic.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position - new Vector3(0, 0.55f, 0));
                line.SetPosition(1,_raycast.point + new Vector2(distanseIn,distanseIn));
            }
        }
        else if (_hookActivated)
        {
            if (isHookedDynamic && 
                Vector3.Distance(line.GetPosition(1), line.GetPosition(0) + new Vector3(-0.1f, 0.1f, 0)) - 
                (line.GetPosition(1).y - _catSprite.transform.position.y) < 0.47
                && !_audioSource.isPlaying && Math.Abs(_catSprite._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0, transform.position - new Vector3(0, 0.55f, 0));
            if (_joint2DDynamic.connectedBody is not null)
                line.SetPosition(1, _joint2DDynamic.connectedBody.transform.position + (Vector3)_joint2DDynamic.connectedAnchor);
        }
        if (_hookActivated && (joystick.Vertical != 0 || joystick.Horizontal != 0) || (_joint2DDynamic.connectedBody is not null && !_joint2DDynamic.connectedBody.gameObject.GetComponent<SpriteRenderer>().enabled))
        {
            _hookActivated = false;
            isHookedDynamic = false;
            _joint2DDynamic.enabled = false;
            line.enabled = false;
        }
        if (isHookedDynamic && _joint2DDynamic.enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            _joint2DDynamic.distance -= scroll;
        }
        #endif
        
    }

    private void GroundHook()
    {
        #if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            target = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            _raycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, maskStatic);
            var difRaycast = Physics2D.Raycast(transform.position, target - transform.position, distanse, maskDynamic);
            if (_raycast.collider is not null &&
                (_raycast.point + new Vector2(distanseIn,distanseIn)).y > _catSprite.transform.position.y &&
                Vector2.Distance(_raycast.point, transform.position)<Vector2.Distance(difRaycast.point, transform.position))
            {
                isHookedStatic = true;
                _joint2DStatic.enabled = true;
                _joint2DStatic.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2DStatic.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2DStatic.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position - new Vector3(0, 0.55f, 0));
                line.SetPosition(1,_raycast.point + new Vector2(distanseIn,distanseIn));
            }
        }
        else if (Input.GetKey(KeyCode.Mouse1))
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
                transform.rotation = new Quaternion(0,180f,0,0);
            else
                transform.rotation = new Quaternion(0,0,0,0);
            if (isHookedStatic && 
                Vector3.Distance(line.GetPosition(1), line.GetPosition(0) + new Vector3(-0.1f, 0.1f, 0)) - 
                (line.GetPosition(1).y - _catSprite.transform.position.y) < 0.47
                && !_audioSource.isPlaying && Math.Abs(_catSprite._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0, transform.position - new Vector3(0, 0.55f, 0));
            if (_joint2DStatic.connectedBody is not null)
                line.SetPosition(1, _joint2DStatic.connectedBody.transform.position + (Vector3)_joint2DStatic.connectedAnchor + new Vector3(0,0.1f,0));
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            isHookedStatic = false;
            _joint2DStatic.enabled = false;
            line.enabled = false;
        }
        if (isHookedStatic && _joint2DStatic.enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0 &&
                !Physics2D.OverlapCircle(_catSprite.checkpointCheck.position + new Vector3(0, 0.3f, 0),
                    _catSprite.distanseCheckpoint, _catSprite.groundLayer))
            {
                _joint2DStatic.distance -= scroll;
                if (writeText != null)
                    writeText.mouseWheelUp.enabled = false;
            }
            
            else if (scroll < 0 &&
                     !(Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius * 1.8f, _catSprite.groundLayer) ||
                       Physics2D.OverlapCircle(_catSprite.groundCheck2.position, _catSprite.groundCheckRadius * 1.8f, _catSprite.groundLayer)))
            {
                if (writeText != null)
                {
                    writeText.final = true;
                    writeText.mouseWheelDown.enabled = false;
                }
                _joint2DStatic.distance -= scroll;
            }
        }
        #elif UNITY_ANDROID
        if (joystick.Horizontal != 0 || joystick.Vertical != 0) _joystickUsedStatic = true;
        if (Math.Abs(joystick.Horizontal) > 0.6f || Math.Abs(joystick.Vertical) > 0.6f)
            _directionJoystickStatic = joystick.Direction;
        if (joystick.Horizontal == 0 && joystick.Vertical == 0 && _joystickUsedStatic)
        {
            _joystickUsedStatic = false;
            _raycast = Physics2D.Raycast(transform.position, transform.position + 
                                                             new Vector3(_directionJoystickStatic.x,
                                                                 _directionJoystickStatic.y,
                                                                 0)*100, distanse, maskStatic);
            var difRaycast = Physics2D.Raycast(transform.position, transform.position + 
                                                                   new Vector3(_directionJoystickStatic.x,
                                                                       _directionJoystickStatic.y,
                                                                       0)*100, distanse, maskDynamic);
            if (_raycast.collider is not null &&
                (_raycast.point + new Vector2(distanseIn,distanseIn)).y > _catSprite.transform.position.y &&
                Vector2.Distance(_raycast.point, transform.position)<Vector2.Distance(difRaycast.point, transform.position))
            {
                _hookActivatedStatic = true;
                isHookedStatic = true;
                _joint2DStatic.enabled = true;
                _joint2DStatic.connectedBody = _raycast.collider.gameObject.GetComponent<Rigidbody2D>();
                _joint2DStatic.connectedAnchor = _raycast.point - new Vector2(_raycast.collider.transform.position.x,
                    _raycast.collider.transform.position.y);
                _joint2DStatic.distance = Vector2.Distance(transform.position, _raycast.point);
                line.enabled = true;
                line.SetPosition(0,transform.position - new Vector3(0, 0.55f, 0));
                line.SetPosition(1,_raycast.point + new Vector2(distanseIn,distanseIn));
            }
        }
        else if (_hookActivatedStatic)
        {
            if (Input.GetAxisRaw("Horizontal") < 0)
                transform.rotation = new Quaternion(0,180f,0,0);
            else
                transform.rotation = new Quaternion(0,0,0,0);
            if (isHookedStatic && 
                Vector3.Distance(line.GetPosition(1), line.GetPosition(0) + new Vector3(-0.1f, 0.1f, 0)) - 
                (line.GetPosition(1).y - _catSprite.transform.position.y) < 0.47
                && !_audioSource.isPlaying && Math.Abs(_catSprite._rb.velocity.x) > 1.5)
            {
                _audioSource.volume = volume;
                _audioSource.PlayOneShot(woosh);
            }
            line.SetPosition(0, transform.position - new Vector3(0, 0.55f, 0));
            if (_joint2DStatic.connectedBody is not null)
                line.SetPosition(1, _joint2DStatic.connectedBody.transform.position + (Vector3)_joint2DStatic.connectedAnchor + new Vector3(0,0.1f,0));
        }
        if (_hookActivatedStatic && (joystick.Vertical != 0 || joystick.Horizontal != 0))
        {
            _hookActivatedStatic = false;
            isHookedStatic = false;
            _joint2DStatic.enabled = false;
            line.enabled = false;
        }
        if (isHookedStatic && _joint2DStatic.enabled)
        {
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll > 0 &&
                !Physics2D.OverlapCircle(_catSprite.checkpointCheck.position + new Vector3(0, 0.3f, 0),
                    _catSprite.distanseCheckpoint, _catSprite.groundLayer))
            {
                _joint2DStatic.distance -= scroll;
                if (writeText != null)
                    writeText.mouseWheelUp.enabled = false;
            }
            
            else if (scroll < 0 &&
                     !(Physics2D.OverlapCircle(_catSprite.groundCheck1.position, _catSprite.groundCheckRadius * 1.8f, _catSprite.groundLayer) ||
                       Physics2D.OverlapCircle(_catSprite.groundCheck2.position, _catSprite.groundCheckRadius * 1.8f, _catSprite.groundLayer)))
            {
                if (writeText != null)
                {
                    writeText.final = true;
                    writeText.mouseWheelDown.enabled = false;
                }
                _joint2DStatic.distance -= scroll;
            }
        }
        #endif
    }
}