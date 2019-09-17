using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform parent;
    public Vector3 rotation;
    public float sensitivity = 1f;
    public float smooth = 1000f;


    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        rotation.x += Input.GetAxis("Horizontal") * sensitivity;
        //rotation.y += Input.GetAxis("Vertical") * sensitivity;

        rotation.y = Mathf.Clamp(rotation.y, -60f, 60f);
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        Quaternion quarts = Quaternion.Euler(rotation.y, rotation.x, 0f);
        parent.rotation = Quaternion.Lerp(parent.rotation, quarts, Time.deltaTime * smooth);

        //// Rotate the cube by converting the angles into a quaternion.
        //Quaternion target = Quaternion.Euler(RotX, 0, RotZ);

        //// Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
