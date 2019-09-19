using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindArea : MonoBehaviour
{
    public float strength = 500f; // Strength stores the value of wind speed
    public Vector3 direction = new Vector3(0,0,1); // in which direction does the wind push?, default is on the z value

    public bool worldSpace = true; // Is the direction given in world space or local space

    private Rigidbody _rigid;

    private void OnTriggerStay(Collider other)
    {
        _rigid = other.attachedRigidbody;
        _rigid.AddForce((worldSpace ? direction : transform.TransformDirection(direction)) * strength);
    }
}
