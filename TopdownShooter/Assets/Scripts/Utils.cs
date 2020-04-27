using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Quaternion RotateY(Vector3 target, Vector3 current)
    {
        Vector3 dir = (target - current).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        lookRot.eulerAngles = new Vector3(0, lookRot.eulerAngles.y, 0);
        return lookRot;
    }
}
