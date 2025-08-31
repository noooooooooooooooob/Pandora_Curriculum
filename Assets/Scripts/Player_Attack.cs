using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public Camera_Follow camera_Follow;
    float isCharging = 0;
    public float maxCharge;

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            isCharging += Time.deltaTime;
            isCharging = Mathf.Clamp(isCharging, 0, maxCharge);
            camera_Follow.PlusCameraSize(isCharging);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isCharging = 0;
            camera_Follow.ResetCameraSize();
        }
    }
}
