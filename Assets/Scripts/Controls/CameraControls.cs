using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    public Transform parent;
    public Transform player1;
    public Vector3 player;
    public Vector3 rotation;
    public float sensitivity = 50f;
    public float smooth = 1000f;

    private Quaternion quarts;

    public bool turnRight;
    public bool turnLeft;


    // Start is called before the first frame update
    void Start()
    {
        parent = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        //rotation.x += Input.GetAxis("Horizontal") * sensitivity;
        //rotation.y += Input.GetAxis("Vertical") * sensitivity;
        bool inputRight = Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        bool inputLeft = Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        //bool inputV = Input.GetButton("Vertical");

        if (inputRight)
        {
            turnRight = true;            
        }

        if (inputLeft)
        {
            turnLeft = true;
        }

        //if (inputV)
        //{
            //turnRight = true;
        //}

        if (turnRight)
        {
            rotation.x += Time.deltaTime * sensitivity;
            //int[] maxAngle = new int[4];
            //for (int i = 0; i <= maxAngle.Length; i++)
            //{
            //    int repeatedAngle = 0;
            //    maxAngle[i] = repeatedAngle + 90;
            //    Debug.Log(maxAngle[i]);
            //    turnRight = false;
            //}

            if (rotation.x >= 90f)
            {
                turnRight = false;
            }
            if(rotation.x == 90f)
            {
                float time = Time.deltaTime;
                if(time > time + 2f)
                {
                    
                }
            }

            //Debug.Log("Call Terus");
        }
        if (turnLeft)
        {
            rotation.x -= Time.deltaTime * sensitivity;

            if (rotation.x <= 0f)
            {
                turnLeft = false;
            }
        }

        rotation.y = Mathf.Clamp(rotation.y, -60f, 60f);
        //rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);

        //else if (rotation.x < 0)
        //{
        //    //quarts = Quaternion.Euler(0, -90f, 0f);
        //}

        quarts = Quaternion.Euler(rotation.y, rotation.x, 0f);
        parent.rotation = Quaternion.Slerp(parent.rotation, quarts, Time.deltaTime * smooth);
        //parent.rotation = Quaternion.FromToRotation(new Vector3(0, 0, 0), rotX);


        //// Rotate the cube by converting the angles into a quaternion.
        //Quaternion target = Quaternion.Euler(RotX, 0, RotZ);

        //// Dampen towards the target rotation
        //transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);
    }
    
    //IEnumerator TurnHorizontal()
    //{
    //    rotation.x += Time.deltaTime * sensitivity;
    //    if (rotation.x >= 90f)
    //    {
    //        rotation.x = 90f;
    //    }

    //    Debug.Log("Calling");

    //    yield return new WaitForEndOfFrame();
    //}

    //void TurnHorizontal1()
    //{
    //    rotation.x += Time.deltaTime * sensitivity;
    //    if (rotation.x >= 90f)
    //    {
    //        rotation.x = 90f;
    //    }
    //}
}
