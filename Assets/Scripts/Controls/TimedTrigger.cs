using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimedTrigger : MonoBehaviour
{
    [SerializeField]
    private float _time = 0f;

    [SerializeField]
    private bool _triggered = true;

    public UnityEvent OnTrigger;

    private float _timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _timer = (_triggered ? 0f : _time);
    }

    // Update is called once per frame
    void Update()
    {
        _timer = Mathf.Max(0f, _timer - Time.deltaTime);

        if ((!_triggered) && (0f == _timer))
        {
            OnTrigger.Invoke();
            _triggered = true;
        }
    }

    public void StartTimer()
    {
        _triggered = false;
        _timer = _time;
    }
}
