using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WorldPlayerInput))]
public class WorldPlayerCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    private WorldPlayerInput input;

    private bool cameraRotationEnabled;
    private float cameraDistance;
    private float yaw;
    private float pitch;
    private float maxRotationSpeed = 0.35f;
    private float stepSize = 1f;
    private Vector2 minMaxClamp = new Vector2(15, 75);
    private Vector2 minMaxZoom = new Vector2(40, 65);

    private void Awake()
    {
        input = GetComponent<WorldPlayerInput>();

        input.OnCameraEnableRotationChange += CameraRotationEnabledChanged;
        input.OnCameraZoom += Zoom;

        // Call this once at start to set the cameraDistance & minMaxClamp
        cameraDistance = minMaxZoom.x;
        Rotate(true);
    }

    private void LateUpdate()
    {
        Rotate();
        FollowTarget();
    }

    public void FollowTarget()
    {
        transform.position = target.position - (transform.forward * cameraDistance);
    }

    public void Rotate(bool force = false)
    {
        if(!cameraRotationEnabled && !force) { return; }
        
        yaw += input.GetCameraRotation().x * maxRotationSpeed;
        pitch -= input.GetCameraRotation().y * maxRotationSpeed;
        pitch = Mathf.Clamp(pitch, minMaxClamp.x, minMaxClamp.y);

        transform.eulerAngles = new Vector3(pitch, yaw);
    }

    public void Zoom()
    {
        float inputValue = input.GetCameraZoom().y / 100f;

        if (Mathf.Abs(inputValue) > 0.1f)
        {
            cameraDistance = cameraDistance - inputValue * stepSize;
            if (cameraDistance < minMaxZoom.x)
            {
                cameraDistance = minMaxZoom.x;
            }
            else if (cameraDistance > minMaxZoom.y)
            {
                cameraDistance = minMaxZoom.y;
            }
        }
    }

    private void CameraRotationEnabledChanged(bool enabled)
    {
        cameraRotationEnabled = enabled;
    }
}
