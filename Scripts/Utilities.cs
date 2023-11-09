using UnityEngine;
using System;

public class Utilities
{
    private static float GetAngle(Vector2 a, Vector2 b)
    {
        
        var ax = a.x; var bx = b.x;
        var ay = a.y; var by = b.y;

        var diag = Vector2.Distance(a, b);

        var yDiff = Math.Abs(by - ay);

        double tanTheta = yDiff / diag;
        var theta = Convert.ToSingle(Math.Asin(tanTheta) * (180f / Math.PI));

        if (ax >= bx && ay >= by)
        {
            theta = 180 + theta;
        }
        else if (ax >= bx)
        {
            theta = 180 - theta;
        }
        else if (ay >= by)
        {
            theta = 360 - theta;
        }

        return theta;
    }
    
    public static void RotateObject(Vector2 origin , Vector2 target, ref GameObject obj)
    {
        Vector2 weaponPos = obj.transform.position;

        var v = target - origin;
        float d1 = Vector2.Distance(target, origin), d2 = Vector2.Distance(weaponPos, origin);

        weaponPos = new Vector2(v.x * d2 / d1, v.y * d2 / d1) + origin;
        obj.transform.position = weaponPos;

        var angle = GetAngle(weaponPos, target);
        
        var look = Quaternion.Euler(0, 0, angle);
        var reset = Quaternion.Euler(0, 0,0);
        obj.transform.rotation = Quaternion.Slerp(reset, look, 1f);
    }
}