using UnityEngine;

public class SceneLoad : MonoBehaviour
{
    public void LoadScene(int idx)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(idx);
    }
}
