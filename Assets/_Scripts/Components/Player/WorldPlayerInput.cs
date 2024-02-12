using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class WorldPlayerInput : MonoBehaviour
{
    public event Action OnSingleClick;
    public event Action OnCameraZoom;
    public event CameraRotationEnabledDelegate OnCameraEnableRotationChange;
    public delegate void CameraRotationEnabledDelegate(bool enabled);

    private CustomInput input;

    private void OnEnable()
    {
        input = new CustomInput();

        input.PlayerOverworld.SingleClickToMove.started += SingleClickToMove;
        input.PlayerOverworld.CameraEnableRotation.started += CameraRotationEnabled;
        input.PlayerOverworld.CameraEnableRotation.canceled += CameraRotationEnabled;
        input.PlayerOverworld.CameraZoom.performed += CameraZoom;
        input.Enable();
    }

    private void OnDisable()
    {
        input.PlayerOverworld.SingleClickToMove.started -= SingleClickToMove;
        input.PlayerOverworld.CameraEnableRotation.started -= CameraRotationEnabled;
        input.PlayerOverworld.CameraEnableRotation.canceled -= CameraRotationEnabled;
        input.PlayerOverworld.CameraZoom.performed -= CameraZoom;
        input.Disable();
    }

    public Vector2 GetCameraRotation()
    {
        Debug.Log(input.PlayerOverworld.CameraRotation.ReadValue<Vector2>());
        return input.PlayerOverworld.CameraRotation.ReadValue<Vector2>();
    }

    public Vector3 GetMousePosition()
    {
        return input.PlayerOverworld.MousePosition.ReadValue<Vector2>();
    }

    public Vector2 GetCameraZoom()
    {
        return input.PlayerOverworld.CameraZoom.ReadValue<Vector2>();
    }

    private void SingleClickToMove(CallbackContext context)
    {
        if (!context.started) { return; }

        OnSingleClick?.Invoke();
    }

    private void CameraRotationEnabled(CallbackContext context)
    {
        Debug.Log("ROTATION CHANGE");
        if (context.started)
        {
            Debug.Log("TRUE");
            OnCameraEnableRotationChange?.Invoke(true);
            return;
        }

        if (context.canceled)
        {
            Debug.Log("FALSE");
            OnCameraEnableRotationChange?.Invoke(false);
            return;
        }
    }

    private void CameraZoom(CallbackContext context)
    {
        OnCameraZoom?.Invoke();
    }
}
