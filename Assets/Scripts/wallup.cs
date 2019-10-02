using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallup : MonoBehaviour
{
    public GameObject wallUp;
    // Start is called before the first frame update
    public void wallUpy()
    {
        wallUp.transform.Translate(0, 50, 0);
    }
}
