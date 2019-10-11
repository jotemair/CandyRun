using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerControlsInput : MonoBehaviour
{
    #region Private Variables

    private PlayerControls _controls = null;
    private Camera _cam = null;

    #endregion

    #region MonoBehaviour Functions

    private void Start()
    {
        _controls = GetComponent<PlayerControls>();
        _cam = Camera.main;
    }

    private void Update()
    {
        // Get the keyboard input
        Vector3 keyboadInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Get the camera rotation along the y axis, and rotate the keyboard input vector
        // This will make it so, that when the player presses forward, it will be forward in relation to the camera and not worldspace
        Quaternion rotate = Quaternion.AngleAxis(_cam.transform.rotation.eulerAngles.y, Vector3.up);
        keyboadInput = rotate * keyboadInput;

        // Pass the camera aligned keyboard input and the jump key state to the player controller script
        _controls.MovementInput = keyboadInput;
        _controls.JumpInput = Input.GetKeyDown(KeyCode.Space);
        _controls.JumpInputHeld = Input.GetKey(KeyCode.Space);
    }

    #endregion
}
