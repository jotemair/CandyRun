using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    #region Private Variables

    private BallSwitch _lastSpawn = null;

    #endregion

    #region Public Funcitions

    // Function to set the last spawnpoint that was activated
    public void SetSpawner(BallSwitch spawner)
    {
        _lastSpawn = spawner;
    }

    // Function to respawn the player
    public void Respawn()
    {
        // Check if we have a spawnpoint referenced
        if (null != _lastSpawn)
        {
            // Try to get the player
            PlayerControls player = FindObjectOfType<PlayerControls>();

            // If we have a player, destroy it
            if (null != player)
            {
                Destroy(player.gameObject);
            }

            // Spawn the player at the last spawn point
            _lastSpawn.Spawn();
        }
    }

    #endregion
}
