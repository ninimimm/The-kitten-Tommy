using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private Transform cat;
    private BoxCollider2D boxCollider;
    
    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        boxCollider.enabled = cat.transform.position.y - 0.9f > transform.position.y;
    }
}