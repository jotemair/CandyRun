using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackBut : MonoBehaviour
{
    public void GameReturn()
    {
        SceneManager.LoadScene(0);
    }
}
