using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerControlsInput : MonoBehaviour
{
    private PlayerControls _controls = null;
    private Camera _cam = null;

    private void Start()
    {
        _controls = GetComponent<PlayerControls>();
        _cam = Camera.main;
    }

    void Update()
    {
        Vector3 keyboadInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        Quaternion rotate = Quaternion.AngleAxis(_cam.transform.rotation.eulerAngles.y, Vector3.up);
        keyboadInput = rotate * keyboadInput;

        _controls.MovementInput = keyboadInput;
        _controls.JumpInput = Input.GetKeyDown(KeyCode.Space);
    }
}
