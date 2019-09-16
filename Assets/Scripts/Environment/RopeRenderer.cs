using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeRenderer : MonoBehaviour
{
    [SerializeField]
    private GameObject _ropePrefab = null;

    private List<(Joint, GameObject)> _ropes = new List<(Joint, GameObject)>();

    public GameObject RopePrefab
    {
        set { _ropePrefab = value; }
    }

    private void Start()
    {
        foreach (var joint in GetComponents<Joint>())
        {
            GameObject _rope = Instantiate<GameObject>(_ropePrefab);
            _ropes.Add((joint, _rope));
        }
    }

    // Update is called once per frame
    void Update()
    {
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

        foreach (var rope in _ropes)
        {
            Transform ropeTrans = rope.Item2.transform;

            Vector3 localAnchorPos = rope.Item1.transform.TransformPoint(rope.Item1.anchor);
            Vector3 otherAnchorPos = rope.Item1.connectedBody.transform.TransformPoint(rope.Item1.connectedAnchor);

            ropeTrans.position = (localAnchorPos + otherAnchorPos) / 2f;
            ropeTrans.rotation = Quaternion.FromToRotation(Vector3.forward, otherAnchorPos - localAnchorPos);
            ropeTrans.localScale = new Vector3(ropeTrans.localScale.x, ropeTrans.localScale.y, Vector3.Distance(otherAnchorPos, localAnchorPos));
        }
    }
}
