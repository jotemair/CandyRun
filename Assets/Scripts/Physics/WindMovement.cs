using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class WindMovement : MonoBehaviour
{
    private Rigidbody _rigid;
    private bool _insideWindArea;
    // Start is called before the first frame update
    void Start()
    {
        _rigid = this.gameObject.GetComponent<Rigidbody>();
        _insideWindArea = false;
    }

    private void FixedUpdate()
    {
        if (_insideWindArea)
        {
            WindArea WindArea = GameObject.FindGameObjectWithTag("WindArea").GetComponent<WindArea>();
            _rigid.AddForce(WindArea.direction * WindArea.strength);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.tag == "WindArea")
        {
            _insideWindArea = true;
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.gameObject.tag == "WindArea")
        {
            _insideWindArea = false;
        }
    }
}
