using UnityEngine;

// Component to create a rope bridge between two points
[RequireComponent(typeof(Rigidbody))]
public class RopeBridge : MonoBehaviour
{
    #region Private Variables

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

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        if ((null == _otherAnchor) || (null == _plankPrefab))
        {
            enabled = false;
            return;
        }
        else
        {
            // If all nevessary variables are set, generate the bridge
            GenerateBridge();
        }
    }

    // Debug draw to show where the bridge will be generated
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

    #endregion

    #region Private Functions

    // Helper function to generate the bridge
    private void GenerateBridge()
    {
        // Get the world position of the center of the two anchors
        Vector3 localAnchor = transform.TransformPoint(new Vector3(0f, 0.5f, 0.5f));
        Vector3 otherAnchor = _otherAnchor.transform.TransformPoint(new Vector3(0f, 0.5f, -0.5f));

        // Get the distance between the two points
        float distance = Vector3.Distance(localAnchor, otherAnchor);

        // Calculate the length of the plank (assuming the unscaled prefab has a length of 1 unity unit)
        float plankLength = Vector3.Distance(_plankPrefab.transform.TransformPoint(new Vector3(0f, 0.5f, 0.5f)), _plankPrefab.transform.TransformPoint(new Vector3(0f, 0.5f, -0.5f)));

        // Calculate how many planks we would need to not have too big gaps in between (gap size only for a fully horizontal bridge, not after it starts to hang)
        int plankCount = Mathf.FloorToInt((distance - _maxGapDistance) / (plankLength + _maxGapDistance));

        // Calculate the gap distance based on the number of planks we will use
        float gapDistance = ((distance - (plankCount * plankLength)) / (plankCount + 1f));

        // Check if we need to add planks at all
        if (1 < plankCount)
        {
            // Keep track of the current and previous planks (or anchors), so that we can add the joint connections
            GameObject currentPlank = gameObject;
            GameObject previousPlank = null;

            // Until we have added the required number of planks
            for (int i = 0; i < plankCount; ++i)
            {
                // Calculate the center position of the next plank
                // There's a gaps and planks alternating, and for the last plank we only go half it's length
                Vector3 position = Vector3.Lerp(localAnchor, otherAnchor, ((gapDistance + plankLength) * (i + 1) - (plankLength / 2f)) / distance);

                // Create a new plank
                previousPlank = currentPlank;
                currentPlank = Instantiate<GameObject>(_plankPrefab, position, transform.rotation);

                // Add the connection between the new and the previous parts
                AddSpringConnections(previousPlank, currentPlank.GetComponent<Rigidbody>());

                // If a rope prefab is set, add a rope renderer component
                if (null != _ropePrefab)
                {
                    RopeRenderer ropeRenderer = previousPlank.AddComponent<RopeRenderer>();
                    ropeRenderer.RopePrefab = _ropePrefab;
                }
            }

            // Connect the last plank to the other anchor
            AddSpringConnections(currentPlank, _otherAnchor);

            // If a rope prefab is set, add a rope renderer component
            if (null != _ropePrefab)
            {
                RopeRenderer ropeRenderer = currentPlank.AddComponent<RopeRenderer>();
                ropeRenderer.RopePrefab = _ropePrefab;
            }
        }
    }

    // Helper function to add the spring joints between two pieces
    private void AddSpringConnections(GameObject from, Rigidbody to)
    {
        // Two joints are added for the left and right side
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

    #endregion
}
