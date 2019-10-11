using UnityEngine;

// Custom gravity module that allows better control over gravity than simply being able to turn it on or off
[RequireComponent(typeof(Rigidbody))]
public class CustomGravity : MonoBehaviour
{
    #region Private Variables

    // Constant for default gravity value
    public const float DEFAULT_GRAVITY = 9.8f;

    [SerializeField]
    private float _gravity = DEFAULT_GRAVITY;

    [SerializeField]
    private Vector3 _direction = -Vector3.up;

    private Rigidbody _rb = null;

    #endregion

    #region Public Properties

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

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rb.AddForce(_direction * _rb.mass * _gravity);
    }

    #endregion
}
