using System.Collections.Generic;
using UnityEngine;

// Component to visually represent a Unity Joint in game (that are normally invisible)
public class RopeRenderer : MonoBehaviour
{
    #region Private Variables

    // We use a prefab to display the rope
    [SerializeField]
    private GameObject _ropePrefab = null;

    // The component renders all joints on the object it's attached to, and there can be multiple ones, so we keep them in a list
    private List<(Joint, GameObject)> _ropes = new List<(Joint, GameObject)>();

    #endregion

    #region Public Properties

    public GameObject RopePrefab
    {
        set { _ropePrefab = value; }
    }

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // Initial variable setup
        foreach (var joint in GetComponents<Joint>())
        {
            GameObject _rope = Instantiate<GameObject>(_ropePrefab);
            _ropes.Add((joint, _rope));
        }
    }

    private void Update()
    {
        // Remove all entries in the list where the joint component was destroyed, and destroy the related rope prefab
        _ropes.RemoveAll(
            rope =>
            {
                bool dead = false;

                if (null == rope.Item1)
                {
                    Destroy(rope.Item2);
                    dead = true;
                }

                return dead;
            }
        );

        // Go through the still active joints in the list
        foreach (var rope in _ropes)
        {
            // Get the transform of the rope
            Transform ropeTrans = rope.Item2.transform;

            // Calculate the anchor points from the joint anchor information
            Vector3 localAnchorPos = rope.Item1.transform.TransformPoint(rope.Item1.anchor);
            Vector3 otherAnchorPos = rope.Item1.connectedBody.transform.TransformPoint(rope.Item1.connectedAnchor);

            // Calculate the new position, rotate and scale of the rope based on the anchor positions
            ropeTrans.position = (localAnchorPos + otherAnchorPos) / 2f;
            ropeTrans.rotation = Quaternion.FromToRotation(Vector3.forward, otherAnchorPos - localAnchorPos);

            // Note, scaling assumes that at the scale of 1, the length of the rope prefab is also 1 unity unit
            ropeTrans.localScale = new Vector3(ropeTrans.localScale.x, ropeTrans.localScale.y, Vector3.Distance(otherAnchorPos, localAnchorPos));
        }
    }

    #endregion
}
