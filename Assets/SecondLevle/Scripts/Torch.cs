using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    [SerializeField] private GameObject _cat;
    [SerializeField] private float burnDistanse;

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, _cat.transform.position) < burnDistanse)
            GetComponentInChildren<Light2D>().enabled = true;
        else
            GetComponentInChildren<Light2D>().enabled = false;
    }
}
