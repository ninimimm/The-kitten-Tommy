using UnityEngine;

public class Clowd : MonoBehaviour
{
    [SerializeField] private float ParallaxEffectMultiplier;
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(ParallaxEffectMultiplier,0);
    }
}
