using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
public class AreaTrigger : MonoBehaviour
{
    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    [SerializeField]
    private string _tag = "Player";

    [SerializeField]
    private bool _checkTag = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((!_checkTag) || (other.CompareTag(_tag)))
        {
            OnTriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if ((!_checkTag) || (other.CompareTag(_tag)))
        {
            OnTriggerExitEvent.Invoke();
        }
    }
}
