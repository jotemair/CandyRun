using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    #region Private Variables

    [SerializeField]
    private Transform _button = null;

    [SerializeField]
    private bool _oneTime = true;

    [SerializeField]
    private Material _onMaterial = null;

    private Material _offMaterial = null;
    private Renderer _renderer = null;

    private bool _activated = false;

    #endregion

    #region Public Variables

    public UnityEvent onTrigger;
    public UnityEvent offTrigger;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // Initial variable setup
        _renderer = _button.GetComponent<Renderer>();
        _offMaterial = _renderer.material;
    }

    private void Update()
    {
        // Check if the button is not active yet, but the button object part has been pressed in
        if (!_activated && (_button.localPosition.y < 0f))
        {
            if (_oneTime)
            {
                // If the button was one time only, disable the Physics movement of the button part
                _button.GetComponent<Rigidbody>().isKinematic = true;
            }

            // Activate the button, and set the appropriate material and invoke the connected events
            _renderer.material = _onMaterial;
            onTrigger.Invoke();
            _activated = true;
        }

        // Check if the button is currently activated, and that the butten has been sufficiently released (it uses a spting component to do this)
        if ((_activated) && (_button.localPosition.y > 0.085f))
        {
            // Deactivate the button, and set the appropriate material and invoke the connected events
            _renderer.material = _offMaterial;
            offTrigger.Invoke();
            _activated = false;
        }
    }

    #endregion
}
