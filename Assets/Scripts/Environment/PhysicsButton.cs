using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField]
    private Transform _button = null;

    [SerializeField]
    private bool _oneTime = true;

    [SerializeField]
    private Material _onMaterial = null;

    private bool _activated = false;

    public UnityEvent onTrigger;
    public UnityEvent offTrigger;

    void Update()
    {
        if (!_activated && (_button.localPosition.y < 0f))
        {
            if (_oneTime)
            {
                _button.GetComponent<Rigidbody>().isKinematic = true;
                _button.GetComponent<Renderer>().material = _onMaterial;
            }

            onTrigger.Invoke();
            _activated = true;
        }

        if ((_activated) && (_button.localPosition.y > 0.085f))
        {
            offTrigger.Invoke();
            _activated = false;
        }
    }
}
