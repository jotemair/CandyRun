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

    [SerializeField]
    private float _airForceMultiplier = 1f;

    [SerializeField]
    private float _coyoteTime = 0f;

    private float _coyoteTimer = 0f;

    private Vector3 _movementInput = Vector3.zero;
    private bool _jumpInput = false;
    private bool _jumpInputHeld = false;

    private Vector3 _contactNormal = Vector3.zero;
    private Vector3 _stickyNormal = Vector3.zero;
    private Vector3 _lowestNormal = Vector3.zero;

    private Rigidbody _rb = null;
    private CustomGravity _g = null;

    private bool _grounded = true;
    private bool _sticking = false;

    private int _storedJumps = 0;

    [SerializeField]
    private float _jumpTimeout = 0.1f;
    private float _jumpTimer = 0f;

    [SerializeField]
    private bool _speedBoost = false;

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

    public bool JumpInputHeld
    {
        set { _jumpInputHeld = value; }
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

        _sticking = _sticking && !_jumpInputHeld;

        _g.Gravity = ((_sticking) ? (_gravityWhileSticking) : (CustomGravity.DEFAULT_GRAVITY));

        Quaternion stickRot = ((_sticking) ? (Quaternion.FromToRotation(Vector3.up, _stickyNormal)) : (Quaternion.identity));
        _rb.AddForce(stickRot * _movementInput.normalized * _moveForce * (_grounded ? 1f : _airForceMultiplier));

        // Debug.DrawRay(transform.position, stickRot * _movementInput.normalized);

        if (_sticking)
        {
            _rb.AddForce(-_stickyNormal * _stickForce);
        }

        _jumpTimer = Mathf.Max(0f, _jumpTimer - Time.deltaTime);
        _coyoteTimer = (_grounded ? _coyoteTime : Mathf.Max(0f, _coyoteTimer - Time.deltaTime));

        if (_jumpInput && (0 < _storedJumps))
        {
            Vector3 lookVector = Vector3.Scale(Camera.main.transform.rotation * Vector3.forward, Vector3.right + Vector3.forward).normalized;
            Vector3 jumpVector = (_speedBoost ? lookVector : (_grounded ? _lowestNormal : ((_canAirjump || (_coyoteTimer > 0f)) ? Vector3.up : Vector3.zero)));

            if (Vector3.zero != jumpVector)
            {
                --_storedJumps;
                _jumpTimer = _jumpTimeout;
                GetComponent<Rigidbody>().AddForce(jumpVector * _jumpForce, ForceMode.Impulse);
            }
        }
        else if ((_grounded || _speedBoost) && (0f == _jumpTimer))
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

        float minContactHeight = float.MaxValue;

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

                if (contact.point.y < minContactHeight)
                {
                    _lowestNormal = contact.normal;
                    minContactHeight = contact.point.y;
                }
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

        _grounded = (0 != contactCount) && (minContactHeight < transform.position.y - 0.2f);
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
