using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class Enemy : MonoBehaviour
{
    private WeaponController weaponController;
    private Queue<Vector3> pointsOnCurve = new Queue<Vector3>();
    private Vector3 destination;
    public Vector3 StoppingPoint;
    public float timeInSeconds = 2f;
    float moveTimer = 0;

    private void Start()
    {
        weaponController = GetComponent<WeaponController>();
    }

    public void InitBezier(Vector3 target)
    {
        pointsOnCurve = BezierCurve.Instance.PointsOnCurve(transform.position, transform.position + (transform.right * 10.0f),
                                                    target, 7.0f);
        destination = pointsOnCurve.Dequeue();
    }

    private void Update()
    {
        weaponController.Shoot(false);
    }

    private void MoveToDestination()
    {
        if (transform.position == destination)
        {
            if (pointsOnCurve.Count > 0)
                destination = pointsOnCurve.Dequeue();
            else
            {
                moveTimer += Time.deltaTime;
                if(moveTimer > timeInSeconds)
                {
                    InitBezier(new Vector3(Random.Range(GameManager.Instance.PlayAreaBounds[2], GameManager.Instance.PlayAreaBounds[3]), 0,
                                        Random.Range(GameManager.Instance.PlayAreaBounds[0], GameManager.Instance.PlayAreaBounds[1])));
                    moveTimer = 0;
                }
                return;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, destination, 5f * Time.fixedDeltaTime);
    }

    private void FixedUpdate()
    {
        MoveToDestination();
        RotateTowardsTarget();
    }

    private bool InsidePlayArea()
    {
        if (transform.position.z > GameManager.Instance?.PlayAreaBounds[0] ||
            transform.position.z < GameManager.Instance?.PlayAreaBounds[1] ||
            transform.position.x < GameManager.Instance?.PlayAreaBounds[2] ||
            transform.position.x > GameManager.Instance?.PlayAreaBounds[3])
        {
            return false;
        }
        else
            return true;
    }

    private void RotateTowardsTarget()
    {
        Quaternion lookRot = Utils.RotateY(GameManager.Instance.playerObj.transform.position, transform.position);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, lookRot, 2.0f * Time.fixedDeltaTime);
    }
}
