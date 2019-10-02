using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class restart : MonoBehaviour
{

    public Collider col; 
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider col)
    {
        if (gameObject.tag == "Player")
        {
            SceneManager.LoadScene(4);
            Debug.Log("restart");
        }
    }

 
}
