using UnityEngine;

public class Player_Method : MonoBehaviour
{
    public static Vector3 GetMouseToRotation(Transform transform)
    {
        float angle;
        Camera cam = Camera.main;
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        float x = mousePos.x - transform.position.x;
        float y = mousePos.y - transform.position.y;
        angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
        return new Vector3(0, 0, angle);
    }
}
