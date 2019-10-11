using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CustomGravity))]
public class PlayerControls : MonoBehaviour
{
    #region Private Variables

    [Header("Movement")]

    // Force multiplier for movement
    [SerializeField]
    private float _moveForce = 1200f;

    [Header("Sticking")]

    // Force multiplier for sticking to specific tagged gameobjects
    [SerializeField]
    private float _stickForce = 1000f;

    // Changed gravity while the player is sticking to something to make it easier to go up walls
    [SerializeField]
    private float _gravityWhileSticking = 5f;

    [Header("Jumping")]

    [SerializeField]
    private float _jumpForce = 800f;

    // Determines if the player can jump even if they aren't grounded
    [SerializeField]
    private bool _canAirjump = false;

    // The maximum number of jumps the player can make before they need to land again to recharge their jumps
    [SerializeField]
    private int _maxJumpCount = 1;

    // Force multiplier applied to movement when the player isn't grounded, to make air movement more difficult
    [SerializeField]
    private float _airForceMultiplier = 1f;

    // Coyote time to allow for grounded jumps for a few frames even after player leaves the ground
    [SerializeField]
    private float _coyoteTime = 0f;

    private float _coyoteTimer = 0f;

    // Inputs for control
    private Vector3 _movementInput = Vector3.zero;
    private bool _jumpInput = false;
    private bool _jumpInputHeld = false;

    // Various surface normals
    private Vector3 _contactNormal = Vector3.zero;
    private Vector3 _stickyNormal = Vector3.zero;
    private Vector3 _lowestNormal = Vector3.zero;

    // Reference to other components on the player gameobject
    private Rigidbody _rb = null;
    private CustomGravity _g = null;

    // Boold indicating current state
    private bool _grounded = true;
    private bool _sticking = false;

    // Number of jumps remaining
    private int _storedJumps = 0;

    // Timer before jumps can replenish after jumping
    // It takes a few frames to leave ground contact after jumping, so this makes sure that the player doesn't immediately recover the jump it just used
    // The jump timeout works as a cooldown if speed boost mode is enabled for jumping
    [SerializeField]
    private float _jumpTimeout = 0.1f;
    private float _jumpTimer = 0f;

    // In speed boost mode, jump direction is based on the camera facing direction, not on surface normals
    // This turns the jump into more of a speed boost in the direction the camera is facing
    [SerializeField]
    private bool _speedBoost = false;

    // A dictionary of instance IDs as keys and associated contact point list as values
    // Used to keep track of the gameobjects the player is in contact with to calculate surface normals
    // The instance ID should be unique and unchangeing for each unique gameobject, at least within one run of the scene,
    //  but that's sufficient for us, as we don't keep this data between scene loads
    private Dictionary<int, List<ContactPoint>> _collisions = new Dictionary<int, List<ContactPoint>>();

    #endregion

    #region public Properties

    // Grounded property, read only
    public bool Grounded
    {
        get { return _grounded; }
    }

    // Sticking property, read only
    public bool Sticking
    {
        get { return _sticking; }
    }

    // Movement input to set
    public Vector3 MovementInput
    {
        set { _movementInput = value; }
    }

    // Jumping input to set, jumping input is used for all the special abilities, not just actual jumping
    public bool JumpInput
    {
        set { _jumpInput = value; }
    }

    // Input to check if the jump button is held
    public bool JumpInputHeld
    {
        set { _jumpInputHeld = value; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _g = GetComponent<CustomGravity>();
    }

    private void FixedUpdate()
    {
        // Update contact normals
        GetNormals();

        // If the jump input is held, sticking is disabled. This helps getting off of sticky surfaces with more player control
        _sticking = _sticking && !_jumpInputHeld;

        // Update gravity based on if we are sticking or not
        _g.Gravity = ((_sticking) ? (_gravityWhileSticking) : (CustomGravity.DEFAULT_GRAVITY));

        // When we are stuck to a surface, the movement input is adjusted to the sticking vector (the normal of the surfaces we are stuck to)
        // So we get the rotation from the straigth up vector to the sticky normal
        Quaternion stickRot = ((_sticking) ? (Quaternion.FromToRotation(Vector3.up, _stickyNormal)) : (Quaternion.identity));

        // We add movement force based on the movement input, and if we are grounded or not
        _rb.AddForce(stickRot * _movementInput.normalized * _moveForce * (_grounded ? 1f : _airForceMultiplier));

        // Add sticking force if enabled
        if (_sticking)
        {
            _rb.AddForce(-_stickyNormal * _stickForce);
        }

        // Update jump and coyote timers
        _jumpTimer = Mathf.Max(0f, _jumpTimer - Time.deltaTime);
        _coyoteTimer = (_grounded ? _coyoteTime : Mathf.Max(0f, _coyoteTimer - Time.deltaTime));

        // Check if jump input is active, and we have stored jumps
        if (_jumpInput && (0 < _storedJumps))
        {
            // Get the camera look vector (x and z direction only), in case it's needed for speed boost
            Vector3 lookVector = Vector3.Scale(Camera.main.transform.rotation * Vector3.forward, Vector3.right + Vector3.forward).normalized;

            // Determine the jump vector
            //   - If using speed boost mode, use look vector
            //   - Otherwise if grounded use the normal of the lowest contact point, this gives better jump direction than using the combined surface normal of all contact points
            //   - Otherwise if we're in the air and we can airjump, or we still have coyote time left, use a simple Vector up
            //   - Otherwise we can't jump and use a zero vector
            Vector3 jumpVector = (_speedBoost ? lookVector : (_grounded ? _lowestNormal : ((_canAirjump || (_coyoteTimer > 0f)) ? Vector3.up : Vector3.zero)));

            // Check if we have a non zero jump vector
            if (Vector3.zero != jumpVector)
            {
                // Reduce the number of stored jumps, and add jump force
                --_storedJumps;
                _jumpTimer = _jumpTimeout;
                GetComponent<Rigidbody>().AddForce(jumpVector * _jumpForce, ForceMode.Impulse);
            }
        }
        else if ((_grounded || _speedBoost) && (0f == _jumpTimer))
        {
            // If we aren't jumping, and we're on the ground, and the jump timer has elapsed, restore the stored jumps
            // Note that in speed boost mode we don't need to be grounded to regain stored jumps, or in that case, speed boosts
            _storedJumps = _maxJumpCount;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Add the contact point of new collisions to the collisions list
        _collisions.Add(collision.collider.gameObject.GetInstanceID(), new List<ContactPoint>(collision.contacts));
    }

    private void OnCollisionStay(Collision collision)
    {
        // Update the contact point list for collision objects we stay in contact with
        _collisions[collision.collider.gameObject.GetInstanceID()] = new List<ContactPoint>(collision.contacts);
    }

    private void OnCollisionExit(Collision collision)
    {
        // Remove collision objects from the collisions list when they exit the collision
        _collisions.Remove(collision.collider.gameObject.GetInstanceID());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + _stickyNormal);

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + _contactNormal);
    }

    #endregion

    #region Private Functions

    // Helper function to get the current contact surface normals
    private void GetNormals()
    {
        // Start a new coutner for the normals
        Vector3 contactNormal = Vector3.zero;
        int contactCount = 0;

        Vector3 stickyNormal = Vector3.zero;
        int stickyCount = 0;

        // Reset the lowest normal, and keep track of the lowest contact point height
        float minContactHeight = float.MaxValue;
        _lowestNormal = Vector3.zero;

        // Go through all objects the player is currently colliding with
        foreach (var collision in _collisions)
        {
            // For each object go through all collision points
            foreach (var contact in collision.Value)
            {
                // If the contact object has the sticky tag, add it to the sticky normal as well
                if (contact.otherCollider.CompareTag("Sticky"))
                {
                    stickyNormal += contact.normal;
                    ++stickyCount;
                }

                // Add together all contact surface normals
                contactNormal += contact.normal;
                ++contactCount;

                // Keep track of the surface normal with the lowest contact point height
                if (contact.point.y < minContactHeight)
                {
                    _lowestNormal = contact.normal;
                    minContactHeight = contact.point.y;
                }
            }
        }

        // Update sticking state and sticking normal
        _sticking = ((0f != _stickForce) && (0 != stickyCount));
        if (_sticking)
        {
            _stickyNormal = stickyNormal.normalized;
        }
        else
        {
            _stickyNormal = Vector3.zero;
        }

        // Update grounded state and contact normal
        // We are only considered grounded if the contact point is low enough compared to the player center position
        // Otherwise it would mean we are only touching simething on the sides, or even with the top of the player, and that's not considered grounded
        _grounded = (0 != contactCount) && (minContactHeight < transform.position.y - 0.2f);
        if (_grounded)
        {
            _contactNormal = contactNormal.normalized;
        }
        else
        {
            _contactNormal = Vector3.zero;
        }
    }

    #endregion
}
