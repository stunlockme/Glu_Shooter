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
    private LayerMask collisionMask;
    private float damage;
    Vector2 offsetSize;

    private void Start()
    {
        offsetSize.x = GetComponent<Renderer>().bounds.size.x * 0.5f;
        offsetSize.y = GetComponent<Renderer>().bounds.size.z * 0.5f;
    }

    public void Init(LayerMask _collisionMask, float _damage)
    {
        collisionMask = _collisionMask;
        damage = _damage;
    }

    /// <summary>
    /// Init bezier points from position to target.
    /// Reset middle point to within playareabounds if it exceeds.
    /// </summary>
    /// <param name="target">destination to reach</param>
    /// <param name="curveDir">direction of bezier points</param>
    public void InitBezier(Vector3 target, Vector3 curveDir)
    {
        //Debug.Log("target-> " + target + ", " + GameManager.Instance.playerObj.transform.position);
        Vector3 middlePoint = transform.position + curveDir * 10.0f;
        if (middlePoint.z > GameManager.Instance?.PlayAreaBounds[0] - offsetSize.y)   //top
        {
            middlePoint = new Vector3(middlePoint.x, middlePoint.y, GameManager.Instance.PlayAreaBounds[0] - offsetSize.y);
        }
        else if (middlePoint.z < GameManager.Instance?.PlayAreaBounds[1] + offsetSize.y)  //bottom
        {
            middlePoint = new Vector3(middlePoint.x, middlePoint.y, GameManager.Instance.PlayAreaBounds[1] + offsetSize.y);
        }
        else if (middlePoint.x < GameManager.Instance?.PlayAreaBounds[2] + offsetSize.x) //left
        {
            middlePoint = new Vector3(GameManager.Instance.PlayAreaBounds[2] + offsetSize.x, middlePoint.y, middlePoint.z);
        }
        else if (middlePoint.x > GameManager.Instance?.PlayAreaBounds[3] - offsetSize.x) //right
        {
            middlePoint = new Vector3(GameManager.Instance.PlayAreaBounds[3] - offsetSize.x, middlePoint.y, middlePoint.z);
        }
        pointsOnCurve = BezierCurve.Instance.PointsOnCurve(transform.position, middlePoint,
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
        //Debug.Log("speed-> " + speed);
        transform.position = Vector3.MoveTowards(transform.position, destination, speed * Time.fixedDeltaTime);
    }

    /// <summary>
    /// check for collision in objects forward direction.
    /// </summary>
    void CheckCollisions()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(transform.position, transform.forward, Color.blue);
        RaycastHit hit;
        float maxDist = 1f;

        if (Physics.Raycast(ray, out hit, maxDist, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    /// <summary>
    /// If object hit Idamageable object, will damage object and disableself.
    /// </summary>
    /// <param name="hit">hit info</param>
    void OnHitObject(RaycastHit hit)
    {
        //Debug.Log(hit.collider.name);
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage, hit);
        }
        gameObject.SetActive(false);
        destination = Vector3.zero;
        gameObject.transform.parent = ObjectPooler.Instance?.PoolParent.transform;
    }

    private void Update()
    {
        AddBackToPool();
        CheckCollisions();
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
}
