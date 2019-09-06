using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerControls))]
public class PlayerControlsInput : MonoBehaviour
{
    private PlayerControls _controls = null;

    private void Start()
    {
        _controls = GetComponent<PlayerControls>();
    }

    void Update()
    {
        Vector3 keyboadInput = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        _controls.MovementInput = keyboadInput;
        _controls.JumpInput = Input.GetKeyDown(KeyCode.Space);
    }
}
