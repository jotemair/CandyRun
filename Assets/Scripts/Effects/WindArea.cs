using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float strength = 1000f;
    public Vector3 direction = new Vector3(0,0,1);

    private Rigidbody _rb;
    private GameObject _player;

    private void Start()
    {
        _player = GameObject.Find("Sphere");
        _rb = _player.GetComponent<Rigidbody>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (_player)
        {
            _rb.AddForce(direction * strength);
        }
    }
}
