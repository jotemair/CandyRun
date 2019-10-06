using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall : MonoBehaviour
{

    public GameObject walldony;
    // Start is called before the first frame update
    public void wally()
    {
      walldony.transform.Translate(0, -50, 0);
    }
}
