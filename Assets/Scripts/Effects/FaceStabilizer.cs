using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceStabilizer : MonoBehaviour
{
    private Transform _root = null;
    private Quaternion _initalRotation = Quaternion.identity;
    private Vector3 _currentDirection = Vector3.zero;
    private Vector3 _startDirection = Vector3.zero;

    void Start()
    {
        _root = transform.parent;
        _currentDirection = transform.localPosition;
        _startDirection = _currentDirection;
        _initalRotation = transform.localRotation;
    }

    void Update()
    {
        Vector3 speed = Vector3.Scale(_root.GetComponent<Rigidbody>().velocity, new Vector3(1f, 0f, 1f));

        if (speed.magnitude > 0.3f)
        {
            speed = speed.normalized * _currentDirection.magnitude;
            _currentDirection = Vector3.Lerp(_currentDirection, speed, Time.deltaTime * 3f).normalized * _currentDirection.magnitude;
        }
        
        transform.localRotation = Quaternion.Inverse(_root.rotation) * _initalRotation * Quaternion.FromToRotation(_startDirection, _currentDirection);
        transform.localPosition = _root.InverseTransformDirection(_currentDirection);
    }
}
