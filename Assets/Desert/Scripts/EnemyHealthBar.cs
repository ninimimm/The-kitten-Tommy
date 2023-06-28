using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] public Transform _target;

    [SerializeField] public float _value;
    private void FixedUpdate() =>
        transform.position = new Vector3(_target.position.x, _target.position.y + _value, _target.position.z);
}
