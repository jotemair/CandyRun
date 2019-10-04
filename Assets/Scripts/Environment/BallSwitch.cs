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
            Vector3 pos = other.transform.position;
            Destroy(other.gameObject);
            Spawn(pos);
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
        Spawn(transform.position);
    }

    private void Spawn(Vector3 position)
    {
        RespawnSystem respawn = FindObjectOfType<RespawnSystem>();
        if (null != respawn)
        {
            respawn.SetSpawner(this);
        }

        Instantiate<GameObject>(_ballPrefab, position, Quaternion.identity);
        _active = false;
    }
}
