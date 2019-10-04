using UnityEngine;

public class RespawnSystem : MonoBehaviour
{
    #region Private Variables

    private BallSwitch _lastSpawn = null;

    #endregion

    #region Public Funcitions

    public void SetSpawner(BallSwitch spawner)
    {
        _lastSpawn = spawner;
    }

    public void Respawn()
    {
        if (null != _lastSpawn)
        {
            PlayerControls player = FindObjectOfType<PlayerControls>();

            if (null != player)
            {
                Destroy(player.gameObject);
            }

            _lastSpawn.Spawn();
        }
    }

    #endregion
}
