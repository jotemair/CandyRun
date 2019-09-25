using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PhysicsButton : MonoBehaviour
{
    [SerializeField]
    private Transform _button = null;

    [SerializeField]
    private bool _oneTime = true;

    private bool _activated = false;

    public UnityEvent onTrigger;
    public UnityEvent offTrigger;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_activated && (_button.localPosition.y < 0f))
        {
            if (_oneTime)
            {
                _button.GetComponent<Rigidbody>().isKinematic = true;
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
