using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    public const float DEFAULT_GRAVITY = 9.8f;

    [SerializeField]
    private float _gravity = DEFAULT_GRAVITY;

    [SerializeField]
    private Vector3 _direction = -Vector3.up;

    private Rigidbody _rb = null;

    public float Gravity
    {
        get { return _gravity; }
        set { _gravity = value; }
    }

    public Vector3 Direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public void FixedUpdate()
    {
        _rb.AddForce(_direction * _rb.mass * _gravity);
    }
}
