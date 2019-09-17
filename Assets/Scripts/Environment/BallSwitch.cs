using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSwitch : MonoBehaviour
{
    [SerializeField]
    private GameObject _ballPrefab = null;

    private bool _active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_active && (other.CompareTag("Player")))
        {
            Destroy(other.gameObject);
            Spawn();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _active = true;
        }
    }

    public void Spawn()
    {
        Instantiate<GameObject>(_ballPrefab, transform.position, Quaternion.identity);
        _active = false;
    }
}
