using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box_reset : MonoBehaviour
{
    //  public Transform Boxo;
    public new GameObject BoxoObjectO;
    public void Reseto_Spawno_yolo()
    {
        for (int i = 1; i != 2; i++)
        {
            Destroy(BoxoObjectO);
            Instantiate(BoxoObjectO, new Vector3(0, 55, 272), Quaternion.identity);
        }
        //    Instantiate(Boxo, new Vector3(2.0F, 0, 0), Quaternion.identity);
    }

    public void EFG()
    {
        Destroy(GameObject.FindWithTag("boxo"));
    }

}
