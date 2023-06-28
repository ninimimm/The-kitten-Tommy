using UnityEngine;

public class parallax : MonoBehaviour
{
    [SerializeField] private Vector2 ParallaxEffectMultiplier;
    private Transform cameraTransform; 
    private Vector3 lastCameraPosition;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * ParallaxEffectMultiplier.x,
            deltaMovement.y * ParallaxEffectMultiplier.y);
        lastCameraPosition = cameraTransform.position;
    }
}
