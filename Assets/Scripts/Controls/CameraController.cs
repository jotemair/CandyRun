using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start()
    {
        _cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (null == _player)
        {
            _player = FindObjectOfType<PlayerControls>();
        }

        if (null != _player)
        {
            if (Input.GetKey(KeyCode.J))
            {
                _angle = (_angle - _angleSpeed) % 360f;
            }

            if (Input.GetKey(KeyCode.L))
            {
                _angle = (_angle + _angleSpeed) % 360f;
            }

            if (Input.GetKey(KeyCode.I))
            {
                _pitch = Mathf.Clamp(_pitch - _pitchSpeed, _pitchMin, _pitchMax);
            }

            if (Input.GetKey(KeyCode.K))
            {
                _pitch = Mathf.Clamp(_pitch + _pitchSpeed, _pitchMin, _pitchMax);
            }

            PositionCamera(_pitch, _angle);
        }
    }

    void PositionCamera(float pitch, float angle)
    {
        float distance = _distance;
        Quaternion rotation = Quaternion.Euler(pitch, angle, 0f);

        Vector3 position = _player.transform.position + rotation * -Vector3.forward * _distance;

        _cam.transform.rotation = rotation;
        _cam.transform.position = position;
    }
}
