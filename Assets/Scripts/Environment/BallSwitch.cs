using UnityEngine;

public class BallSwitch : MonoBehaviour
{
    #region  Private Variables

    [SerializeField]
    private GameObject _ballPrefab = null;

    private bool _active = true;

    #endregion

    #region MonoBehaviour Functions

    private void OnTriggerEnter(Collider other)
    {
        // Check if the area is active, and that the object entering has the player tag
        if (_active && (other.CompareTag("Player")))
        {
            // Get the position, destroy the player, and spawn a new player in the position of the previous one
            Vector3 pos = other.transform.position;
            Destroy(other.gameObject);
            Spawn(pos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If the player leaves the spawn area, reactivate it
            _active = true;
        }
    }

    #endregion

    #region Public Functions

    // Public function to spawn player at the center of the switch area
    public void Spawn()
    {
        Spawn(transform.position);
    }

    #endregion

    #region Private Functions

    // Private function to spawn player at specific position
    private void Spawn(Vector3 position)
    {
        // If a respawn system is found, set this spawn area, as the last spawn
        RespawnSystem respawn = FindObjectOfType<RespawnSystem>();
        if (null != respawn)
        {
            respawn.SetSpawner(this);
        }

        // Instantiate a new player gameobject
        Instantiate<GameObject>(_ballPrefab, position, Quaternion.identity);

        // Deactivate the spawn area, so that it's not triggered by the newly spawned player
        _active = false;
    }

    #endregion
}
