using UnityEngine;

// The players are playing as balls that rotate, but also have a face that looks in the general direction of travel
// This class updates the rotation and position of the face, so that it's not influenced by the rolling of the player body
public class FaceStabilizer : MonoBehaviour
{
    #region Private Variables

    private Transform _root = null;
    private Quaternion _initalRotation = Quaternion.identity;
    private Vector3 _currentDirection = Vector3.zero;
    private Vector3 _startDirection = Vector3.zero;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // Setting initial values
        _root = transform.parent;
        _currentDirection = transform.localPosition;
        _startDirection = _currentDirection;
        _initalRotation = transform.localRotation;
    }

    private void Update()
    {
        // Get the xz speed of the root object
        Vector3 speed = Vector3.Scale(_root.GetComponent<Rigidbody>().velocity, Vector3.right + Vector3.forward);

        // Check if we are above a certin speed value
        if (speed.magnitude > 0.3f)
        {
            // Update the speed vector, so that it still points in the speed direction, but has the magnitude of the current direction
            speed = speed.normalized * _currentDirection.magnitude;

            // Update the current direction vector, so that it starts to face the speed vector, but while maintaining magnitude
            _currentDirection = Vector3.Lerp(_currentDirection, speed, Time.deltaTime * 3f).normalized * _currentDirection.magnitude;
        }
        
        // Update the transform so that it faces in the direction given by the currentDirection vector regardless of the rotation of the root object
        transform.localRotation = Quaternion.Inverse(_root.rotation) * _initalRotation * Quaternion.FromToRotation(_startDirection, _currentDirection);
        transform.localPosition = _root.InverseTransformDirection(_currentDirection);
    }

    #endregion
}
