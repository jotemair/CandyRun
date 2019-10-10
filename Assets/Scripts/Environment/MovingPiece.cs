using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPiece : MonoBehaviour
{
    [SerializeField]
    private Transform _movingPiece = null;

    [SerializeField]
    private float _moveSpeed = 0.1f;

    [SerializeField]
    private float _rotationSpeed = 0.1f;

    [SerializeField]
    private List<Transform> _positions = new List<Transform>();

    [SerializeField]
    private bool _loop = true;

    [SerializeField]
    private bool _active = true;

    [SerializeField]
    private bool _reverse = false;

    [SerializeField]
    private int _targetPosIdx = 0;

    private Vector3 _velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (0 == _positions.Count)
        {
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            _movingPiece.position = Vector3.SmoothDamp(_movingPiece.position, _positions[_targetPosIdx].position, ref _velocity, Time.deltaTime, _moveSpeed);
            _movingPiece.rotation = Quaternion.RotateTowards(_movingPiece.rotation, _positions[_targetPosIdx].rotation, _rotationSpeed);

            bool posApproxEqual = false;
            {
                Vector3 piecePos = _movingPiece.position;
                Vector3 targetPos = _positions[_targetPosIdx].position;

                posApproxEqual = Mathf.Approximately(0f, Vector3.Distance(piecePos, targetPos));
            }

            bool rotApproxEqual = false;
            {
                Quaternion pieceRot = _movingPiece.rotation;
                Quaternion targetRot = _positions[_targetPosIdx].rotation;

                rotApproxEqual = Mathf.Approximately(0f, Quaternion.Angle(pieceRot, targetRot));
            }

            if (posApproxEqual && rotApproxEqual)
            {
                StepTargetIdx();
            }
        }
    }

    private void StepTargetIdx()
    {
        _targetPosIdx = ((_loop) ? ((_targetPosIdx + (_reverse ? -1 : 1)) % _positions.Count) : Mathf.Clamp((_targetPosIdx + (_reverse ? -1 : 1)), 0, _positions.Count - 1));
    }

    public void On()
    {
        _active = true;
    }

    public void Off()
    {
        _active = false;
    }

    public void Toggle()
    {
        _active = !_active;
    }

    public void Reverse()
    {
        _reverse = !_reverse;
        StepTargetIdx();
    }
}
