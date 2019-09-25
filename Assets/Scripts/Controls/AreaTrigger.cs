using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class AreaTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent.Invoke();
    }
}
