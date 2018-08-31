using System;
using UnityEngine;

public static class PhysicHelper
{
    public static void RadialRaycasts(Vector3 pos, int mask, float raysToShoot, float scanDistance, Action<float> storeRay, float startDistance = 0)
    {
        float angle = 0;
        for (int i = 0; i < raysToShoot; i++)
        {
            float x = Mathf.Sin(angle);
            float z = Mathf.Cos(angle);
            angle += 2 * Mathf.PI / raysToShoot;

            Vector3 start = new Vector3(pos.x + x * startDistance, 0, pos.z + z * startDistance);

            Vector3 dir = new Vector3(x, 0, z);
            RaycastHit hit;
            float result = -1;
            if (Physics.Raycast(start, dir, out hit, scanDistance, mask))
            {
                result = 1 - (hit.distance / scanDistance);

                Debug.DrawLine(start, hit.point, Color.red);
            }

            storeRay(result);
        }
    }
}
