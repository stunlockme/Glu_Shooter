using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10;
    public Vector2 target;
    private Queue<Vector3> pointsOnCurve = new Queue<Vector3>();
    private Vector3 lineStart;
    public Vector3 destination;
    private Vector3[] curveDirs;

    private void Awake()
    {
        curveDirs = new Vector3[]
        {
            transform.right,
            -transform.right
        };
    }

    public void InitBezier(Vector3 target, Vector3 curveDir)
    {
        //Debug.Log("target-> " + target + ", " + GameManager.Instance.playerObj.transform.position);       
        pointsOnCurve = BezierCurve.Instance.PointsOnCurve(transform.position, transform.position + (curveDir * 10.0f),
            target, 7.0f);
        destination = pointsOnCurve.Dequeue();
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void MoveToDestination()
    {
        if (transform.position == destination)
        {
            if (pointsOnCurve.Count > 0)
                destination = pointsOnCurve.Dequeue();
            else
            {
                gameObject.SetActive(false);
                destination = Vector3.zero;
                gameObject.transform.parent = ObjectPooler.Instance?.PoolParent.transform;
            }
            //Debug.Log("Destination-> " + destination);
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, 10f * Time.fixedDeltaTime);
    }

    private void Update()
    {
        AddBackToPool();
        if(destination == Vector3.zero)
            transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void FixedUpdate()
    {
        if (destination != Vector3.zero)
        {
            MoveToDestination();
        }
    }

    /// <summary>
    /// Add this object back to objectpool if outside playarea.
    /// </summary>
    private void AddBackToPool()
    {
        if(transform.position.z > GameManager.Instance?.PlayAreaBounds[0] ||
            transform.position.z < GameManager.Instance?.PlayAreaBounds[1] ||
            transform.position.x < GameManager.Instance?.PlayAreaBounds[2] ||
            transform.position.x > GameManager.Instance?.PlayAreaBounds[3])
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = ObjectPooler.Instance?.PoolParent.transform;
        }
    }

    private bool IsOutofPlayArea()
    {
        if (transform.position.z > GameManager.Instance?.PlayAreaBounds[0] ||
            transform.position.z < GameManager.Instance?.PlayAreaBounds[1] ||
            transform.position.x < GameManager.Instance?.PlayAreaBounds[2] ||
            transform.position.x > GameManager.Instance?.PlayAreaBounds[3])
        {
            return true;
        }
        else
            return false;
    }
}
