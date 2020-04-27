using UnityEngine;

public static class Bezier
{
    /// <summary>
    /// Interpolate between first and middle point, also between middle and last point.
    /// </summary>
    /// <param name="p0">first point</param>
    /// <param name="p1">middle point</param>
    /// <param name="p2">last point</param>
    /// <param name="t">fraction of journey</param>
    /// <returns>point</returns>
    public static Vector3 GetPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
    }
}
