using UnityEngine;
using System.Collections;

public class Static2DCollisonHandler : MonoBehaviour {

	public static bool CheckCollison(Vector3 i_direction, Vector3 i_lastPosition, out RaycastHit2D ray, int laserDistance)
    {
        ray = new RaycastHit2D();
     
         ray= Physics2D.Raycast(i_lastPosition, i_direction, laserDistance);
        if (ray.collider==null|| ray.transform.gameObject.layer==5)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}
