using UnityEngine;

public class Move : MonoBehaviour
{
    [SerializeField] public float ParallaxEffectMultiplier;
    // Update is called once per frame
    void Update() => transform.position += new Vector3(ParallaxEffectMultiplier*Time.deltaTime,0);
}