using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Private Variables

    private Camera _cam = null;

    private PlayerControls _player = null;

    [SerializeField]
    private float _distance = 10f;

    [SerializeField]
    private float _angleSpeed = 5f;

    [SerializeField]
    private float _pitchSpeed = 5f;

    [SerializeField]
    private float _pitchMin = -5f;

    [SerializeField]
    private float _pitchMax = 50f;

    private float _pitch = 0f;

    private float _angle = 0f;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _cam = Camera.main;
    }

    private void Update()
    {
        // Try to get the player if we don't have a reference
        // The player doesn't always exists, and at times the current one is destroyed and a new one is made, so we need to constantly check if the reference is still valid
        if (null == _player)
        {
            _player = FindObjectOfType<PlayerControls>();
        }

        // Update the camera only if we have a player reference
        if (null != _player)
        {
            // The J and L keys rotate the camera around the player, the angle value can go from 0 to 360 and loops

            if (Input.GetKey(KeyCode.J))
            {
                _angle = (_angle - _angleSpeed) % 360f;
            }

            if (Input.GetKey(KeyCode.L))
            {
                _angle = (_angle + _angleSpeed) % 360f;
            }


            // The I and  K keys change the camera pitch, the pitch value is clamped between a min and max

            if (Input.GetKey(KeyCode.I))
            {
                _pitch = Mathf.Clamp(_pitch - _pitchSpeed, _pitchMin, _pitchMax);
            }

            if (Input.GetKey(KeyCode.K))
            {
                _pitch = Mathf.Clamp(_pitch + _pitchSpeed, _pitchMin, _pitchMax);
            }

            // Position the camera based on the current values
            PositionCamera(_pitch, _angle);
        }
    }

    #endregion

    #region Private Functions

    // Helper function to position the camera with the given angle and pitch to look at the player
    private void PositionCamera(float pitch, float angle)
    {
        // Get the rotation required to get the given angle and pitch
        Quaternion rotation = Quaternion.Euler(pitch, angle, 0f);

        // Rotate the back vector and multiply it by distance to get the position of the camera from the player
        // Add that to the player's world position to get the camera world position
        Vector3 position = _player.transform.position + rotation * Vector3.back * _distance;

        // Set the camera position and rotation
        _cam.transform.rotation = rotation;
        _cam.transform.position = position;
    }

    #endregion
}
