using UnityEngine;
using System.Collections;

public class RotateController : MonoBehaviour
{

    private float baseAngle = 0.0f;
    public static bool pressOnRing = false;

    void OnMouseDown()
    {
        pressOnRing = true;

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        baseAngle = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg;
        baseAngle -= Mathf.Atan2(transform.right.y, transform.right.x) * Mathf.Rad2Deg;

    }

    void OnMouseUp()
    {
        pressOnRing = false;
    }

    void OnMouseDrag()
    {

        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        pos = Input.mousePosition - pos;
        float ang = Mathf.Atan2(pos.y, pos.x) * Mathf.Rad2Deg - baseAngle;
        transform.rotation = Quaternion.AngleAxis(ang, Vector3.forward);

    }
}