using UnityEngine;
using UnityEngine.Events;

// Class to set up timed events
public class TimedTrigger : MonoBehaviour
{
    #region Public Variables

    public UnityEvent OnTrigger;

    #endregion

    #region Private Variables

    [SerializeField]
    private float _time = 0f;

    [SerializeField]
    private bool _triggered = true;

    private float _timer = 0f;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        // If the timer is set to be already triggered, set the timer to zero, otherwise start the timer
        _timer = (_triggered ? 0f : _time);
    }

    private void Update()
    {
        // Update timer
        _timer = Mathf.Max(0f, _timer - Time.deltaTime);

        // If the timer reaches zero, and we aren't triggered yet
        if ((!_triggered) && (0f == _timer))
        {
            // Invoke the connected event, and set the timer to be triggered
            OnTrigger.Invoke();
            _triggered = true;
        }
    }

    #endregion

    #region Public Functions

    // Function to (re)start the timer
    public void StartTimer()
    {
        _triggered = false;
        _timer = _time;
    }

    #endregion
}
