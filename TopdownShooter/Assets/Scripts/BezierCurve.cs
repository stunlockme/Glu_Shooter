using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurve : Singleton<BezierCurve>
{
    public Vector3[] points;
    public void Reset()
    {
        points = new Vector3[]
        {
            new Vector3(1f, 0f, 0f),
            new Vector3(2f, 0f, 0f),
            new Vector3(3f, 0f, 0f)
        };
    }

    public Vector3 GetPoint(float t)
    {
        return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], t));
    }

    /// <summary>
    /// Interpolates between the three points
    /// </summary>
    /// <param name="v0">first point</param>
    /// <param name="v1">middle point</param>
    /// <param name="v2">last point</param>
    /// <param name="n">number of points on curve</param>
    /// <returns>queue of points on the curve</returns>
    public Queue<Vector3> PointsOnCurve(Vector3 v0, Vector3 v1, Vector3 v2, float n)
    {
        points = new Vector3[]
        {
            v0,
            v1,
            v2
        };
        Queue<Vector3> pointsOnCurve = new Queue<Vector3>();
        Vector3 lineStart;
        lineStart = GetPoint(0f);
        //float n = 7f;
        for (int i = 1; i <= (int)n; i++)
        {
            Vector3 lineEnd = GetPoint(i / n);
            lineStart = lineEnd;
            pointsOnCurve.Enqueue(lineStart);
        }
        return pointsOnCurve;
    }
}