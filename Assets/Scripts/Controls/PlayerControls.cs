using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CustomGravity))]
public class PlayerControls : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField]
    private float _moveForce = 1200f;

    [Header("Sticking")]

    [SerializeField]
    private float _stickForce = 1000f;

    [SerializeField]
    private float _gravityWhileSticking = 5f;

    [Header("Jumping")]

    [SerializeField]
    private float _jumpForce = 800f;

    [SerializeField]
    private bool _canAirjump = false;

    [SerializeField]
    private int _maxJumpCount = 1;

    private Vector3 _movementInput = Vector3.zero;
    private bool _jumpInput = false;

    private Vector3 _contactNormal = Vector3.zero;
    private Vector3 _stickyNormal = Vector3.zero;

    private Rigidbody _rb = null;
    private CustomGravity _g = null;

    private bool _grounded = true;
    private bool _sticking = false;

    private int _storedJumps = 0;

    private float _jumpTimeout = 0.1f;
    private float _jumpTimer = 0f;

    private Dictionary<int, List<ContactPoint>> _collisions = new Dictionary<int, List<ContactPoint>>();

    public bool Grounded
    {
        get { return _grounded; }
    }

    public bool Sticking
    {
        get { return _sticking; }
    }

    public Vector3 MovementInput
    {
        set { _movementInput = value; }
    }

    public bool JumpInput
    {
        set { _jumpInput = value; }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + _stickyNormal);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _contactNormal);
    }

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _g = GetComponent<CustomGravity>();
    }

    private void FixedUpdate()
    {
        GetNormals();

        _g.Gravity = ((_sticking) ? (_gravityWhileSticking) : (CustomGravity.DEFAULT_GRAVITY));

        _rb.AddForce(_movementInput * _moveForce);

        if (_sticking)
        {
            _rb.AddForce(-_stickyNormal * _stickForce);
        }

        _jumpTimer = Mathf.Max(0f, _jumpTimer - Time.deltaTime);

        if (_jumpInput && (0 < _storedJumps))
        {
            --_storedJumps;
            _jumpTimer = _jumpTimeout;

            Vector3 jumpVector = (_grounded ? _contactNormal : (_canAirjump ? Vector3.up : Vector3.zero));

            GetComponent<Rigidbody>().AddForce(jumpVector * _jumpForce, ForceMode.Impulse);
        }
        else if ((_grounded) && (0f == _jumpTimer))
        {
            _storedJumps = _maxJumpCount;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        _collisions.Add(collision.collider.gameObject.GetInstanceID(), new List<ContactPoint>(collision.contacts));
    }

    private void OnCollisionStay(Collision collision)
    {
        _collisions[collision.collider.gameObject.GetInstanceID()] = new List<ContactPoint>(collision.contacts);
    }

    private void OnCollisionExit(Collision collision)
    {
        _collisions.Remove(collision.collider.gameObject.GetInstanceID());
    }

    private void GetNormals()
    {
        Vector3 contactNormal = Vector3.zero;
        int contactCount = 0;

        Vector3 stickyNormal = Vector3.zero;
        int stickyCount = 0;

        foreach (var collision in _collisions)
        {
            foreach (var contact in collision.Value)
            {
                if (contact.otherCollider.CompareTag("Sticky"))
                {
                    stickyNormal += contact.normal;
                    ++stickyCount;
                }

                contactNormal += contact.normal;
                ++contactCount;
            }
        }

        _sticking = ((0f != _stickForce) && (0 != stickyCount));
        if (_sticking)
        {
            _stickyNormal = stickyNormal / ((float)stickyCount);
        }
        else
        {
            _stickyNormal = Vector3.zero;
        }

        _grounded = (0 != contactCount);
        if (_grounded)
        {
            _contactNormal = contactNormal / ((float)contactCount);
        }
        else
        {
            _contactNormal = Vector3.zero;
        }
    }
}
