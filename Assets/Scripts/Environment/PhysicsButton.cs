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

    private Material _offMaterial = null;
    private Renderer _renderer = null;

    private bool _activated = false;

    public UnityEvent onTrigger;
    public UnityEvent offTrigger;

    private void Start()
    {
        _renderer = _button.GetComponent<Renderer>();
        _offMaterial = _renderer.material;
    }

    void Update()
    {
        if (!_activated && (_button.localPosition.y < 0f))
        {
            if (_oneTime)
            {
                _button.GetComponent<Rigidbody>().isKinematic = true;
            }

            _renderer.material = _onMaterial;
            onTrigger.Invoke();
            _activated = true;
        }

        if ((_activated) && (_button.localPosition.y > 0.085f))
        {
            _renderer.material = _offMaterial;
            offTrigger.Invoke();
            _activated = false;
        }
    }
}
