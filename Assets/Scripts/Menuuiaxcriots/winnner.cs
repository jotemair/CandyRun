﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class winnner : MonoBehaviour
{
    public Collider col;
    // Start is called before the first frame update
    public void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(4);
            Debug.Log("win");
        }
    }
}