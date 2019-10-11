using UnityEngine;
using UnityEngine.Events;

//Simple class to set up events triggered by the player, or other objects entering a certain area
[RequireComponent(typeof(BoxCollider))]
public class AreaTrigger : MonoBehaviour
{
    #region Public Variables

    public UnityEvent OnTriggerEnterEvent;
    public UnityEvent OnTriggerExitEvent;

    #endregion

    #region Private Variables

    [SerializeField]
    private string _tag = "Player";

    [SerializeField]
    private bool _checkTag = false;

    #endregion

    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        // If we don't need to check the tag, or a collider with the proper tag entered, trigger the connected event
        if ((!_checkTag) || (other.CompareTag(_tag)))
        {
            OnTriggerEnterEvent.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // If we don't need to check the tag, or a collider with the proper tag exited, trigger the connected event
        if ((!_checkTag) || (other.CompareTag(_tag)))
        {
            OnTriggerExitEvent.Invoke();
        }
    }

    #endregion
}
