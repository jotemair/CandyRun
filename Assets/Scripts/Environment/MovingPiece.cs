using System.Collections.Generic;
using UnityEngine;

// Class to greate moving/rotating pieces in the environment that can also be affected by events to cahnge their movement
public class MovingPiece : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private Transform _movingPiece = null;

    [SerializeField]
    private float _moveSpeed = 0.1f;

    [SerializeField]
    private float _rotationSpeed = 0.1f;

    // List of transforms to move between
    [SerializeField]
    private List<Transform> _positions = new List<Transform>();

    // Whether we should loop through the position list, or stop at the end
    [SerializeField]
    private bool _loop = true;

    // Sets if the object is active and moving, or not
    [SerializeField]
    private bool _active = true;

    // Sets the direction we go through the positions
    [SerializeField]
    private bool _reverse = false;

    // The index of the next position to go to
    [SerializeField]
    private int _targetPosIdx = 0;

    private Vector3 _velocity = Vector3.zero;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // If no points are set, disable the script
        if (0 == _positions.Count)
        {
            enabled = false;
        }
    }

    private void Update()
    {
        // Check if the moving piece is active or not
        if (_active)
        {
            // Smothly move and rotate towards the position in the list that's the current target
            _movingPiece.position = Vector3.SmoothDamp(_movingPiece.position, _positions[_targetPosIdx].position, ref _velocity, Time.deltaTime, _moveSpeed);
            _movingPiece.rotation = Quaternion.RotateTowards(_movingPiece.rotation, _positions[_targetPosIdx].rotation, _rotationSpeed);

            // Check if we are roughly in the right position
            // We use the distance between the current and target position
            bool posApproxEqual = false;
            {
                Vector3 piecePos = _movingPiece.position;
                Vector3 targetPos = _positions[_targetPosIdx].position;

                posApproxEqual = Mathf.Approximately(0f, Vector3.Distance(piecePos, targetPos));
            }

            // Check if we are roughly in the right orientation
            // Using the angle between are current and target position gives good results
            bool rotApproxEqual = false;
            {
                Quaternion pieceRot = _movingPiece.rotation;
                Quaternion targetRot = _positions[_targetPosIdx].rotation;

                rotApproxEqual = Mathf.Approximately(0f, Quaternion.Angle(pieceRot, targetRot));
            }

            // If we are roughly in the right position and orientation, get the index of the next target position
            if (posApproxEqual && rotApproxEqual)
            {
                StepTargetIdx();
            }
        }
    }

    #endregion

    #region Private Functions

    // Helper function to step the target index
    private void StepTargetIdx()
    {
        // The next index is one more or one less than the current one, depending on whether or not we are in reverse
        // And if we are looping, we use modulo, if we're not, we use clamp on the index
        _targetPosIdx = ((_loop) ? ((_targetPosIdx + (_reverse ? -1 : 1)) % _positions.Count) : Mathf.Clamp((_targetPosIdx + (_reverse ? -1 : 1)), 0, _positions.Count - 1));
    }

    #endregion

    #region Public Functions

    // Function to activate the moving piece
    public void On()
    {
        _active = true;
    }

    // Function to deactivate the moving piece
    public void Off()
    {
        _active = false;
    }

    // Function to toggle the active state of the moving piece
    public void Toggle()
    {
        _active = !_active;
    }

    // Function to reverse the order we traverse the positions, it also immediately sets a new target based on the changed traversal order
    public void Reverse()
    {
        _reverse = !_reverse;
        StepTargetIdx();
    }

    #endregion
}
