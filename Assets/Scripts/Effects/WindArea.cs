using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float strength = 1000f; // Strength stores the value of wind speed
    public Vector3 direction = new Vector3(0,0,1); // in which direction does the wind push?, default is on the z value

    private Rigidbody _rb; // _rb stores player rigidbody
    private GameObject _player; //_player stores player game object

    private void Start()
    {
        _player = GameObject.Find("Sphere"); // Get player reference
        _rb = _player.GetComponent<Rigidbody>(); // Get player rigidbody reference
    }

    private void OnTriggerStay(Collider other) // when something is inside the wind area
    {
        if (_player) // if it is player
        {
            _rb.AddForce(direction * strength); // Add strength and direction to rigidbody. 
        }
    }
}
