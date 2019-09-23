using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform parent;
    public Vector3 rotation;
    public float sensitivity = 1f;
    public float smooth = 1000f;

    private Quaternion quarts;

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

        if (rotation.x > 0)
        {
            //Quaternion.FromToRotation(new Vector3(0, 0, 0), rotX);
        }
        //else if (rotation.x < 0)
        //{
        //    //quarts = Quaternion.Euler(0, -90f, 0f);
        //}

        quarts = Quaternion.Euler(0f, rotation.x, 0f);
        parent.rotation = Quaternion.Slerp(parent.rotation, quarts, Time.deltaTime * smooth);
        //parent.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 0), rotX);


        //// Rotate the cube by converting the angles into a quaternion.
        //Quaternion target = Quaternion.Euler(RotX, 0, RotZ);

        //// Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
}
