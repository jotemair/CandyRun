using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RopeBridge : MonoBehaviour
{
    [SerializeField]
    private Rigidbody _otherAnchor = null;

    [SerializeField]
    private GameObject _plankPrefab = null;

    [SerializeField]
    private float _maxGapDistance = 0.2f;

    [SerializeField]
    private float _spring = 1000f;

    [SerializeField]
    private float _breakForce = float.PositiveInfinity;

    [SerializeField]
    private GameObject _ropePrefab = null;

    private void OnDrawGizmos()
    {
        if (null != _otherAnchor)
        {
            for (int i = 0; i < 2; ++i)
            {
                Vector3 localAnchor = (transform.TransformPoint(new Vector3(-0.5f + (float)(i), 0.5f,  0.5f)));
                Vector3 otherAnchor = (_otherAnchor.transform.TransformPoint(new Vector3(-0.5f + (float)(i), 0.5f, -0.5f)));

                Gizmos.color = new Color(0.5f, 0.25f, 0.25f);
                Gizmos.DrawSphere(localAnchor, 0.025f);
                Gizmos.DrawSphere(otherAnchor, 0.025f);

                Gizmos.color = Color.green;
                Gizmos.DrawLine(localAnchor, otherAnchor);
            }
        }
    }

    private void Start()
    {
        if ((null == _otherAnchor) || (null == _plankPrefab))
        {
            enabled = false;
            return;
        }
        else
        {
            GenerateBridge();
        }
    }

    private void GenerateBridge()
    {
        Vector3 localAnchor = transform.TransformPoint(new Vector3(0f, 0.5f, 0.5f));
        Vector3 otherAnchor = _otherAnchor.transform.TransformPoint(new Vector3(0f, 0.5f, -0.5f));

        float distance = Vector3.Distance(localAnchor, otherAnchor);
        float plankLength = Vector3.Distance(_plankPrefab.transform.TransformPoint(new Vector3(0f, 0.5f, 0.5f)), _plankPrefab.transform.TransformPoint(new Vector3(0f, 0.5f, -0.5f)));
        int plankCount = Mathf.FloorToInt((distance - _maxGapDistance) / (plankLength + _maxGapDistance));
        float gapDistance = ((distance - (plankCount * plankLength)) / (plankCount + 1f));

        if (1 < plankCount)
        {
            GameObject currentPlank = gameObject;
            GameObject previousPlank = null;

            for (int i = 0; i < plankCount; ++i)
            {
                Vector3 position = Vector3.Lerp(localAnchor, otherAnchor, ((gapDistance + plankLength) * (i + 1) - (plankLength / 2f)) / distance);
                previousPlank = currentPlank;
                currentPlank = Instantiate<GameObject>(_plankPrefab, position, transform.rotation);
                AddSpringConnections(previousPlank, currentPlank.GetComponent<Rigidbody>());

                if (null != _ropePrefab)
                {
                    RopeRenderer ropeRenderer = previousPlank.AddComponent<RopeRenderer>();
                    ropeRenderer.RopePrefab = _ropePrefab;
                }
            }

            AddSpringConnections(currentPlank, _otherAnchor);

            if (null != _ropePrefab)
            {
                RopeRenderer ropeRenderer = currentPlank.AddComponent<RopeRenderer>();
                ropeRenderer.RopePrefab = _ropePrefab;
            }
        }
    }

    private void AddSpringConnections(GameObject from, Rigidbody to)
    {
        for (int i = 0; i < 2; ++i)
        {
            SpringJoint joint = from.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = new Vector3(-0.5f + (float)(i), 0.5f, 0.5f);
            joint.connectedAnchor = new Vector3(-0.5f + (float)(i), 0.5f, -0.5f);
            joint.connectedBody = to;
            joint.spring = _spring;
            joint.breakForce = _breakForce;
        }
    }
}
