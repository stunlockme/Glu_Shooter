using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class Enemy : LivingEntity
{
    public new string tag;
    private WeaponController weaponController;
    private Queue<Vector3> pointsOnCurve = new Queue<Vector3>();
    private Vector3 destination;
    //public Vector3 StoppingPoint;
    public float timeInSeconds = 2f;
    float moveTimer = 0;
    Vector2 offsetSize;
    public int pointsAwardedOnDeath;

    protected override void Start()
    {
        base.Start();
        weaponController = GetComponent<WeaponController>();
        offsetSize.x = GetComponent<Renderer>().bounds.size.x * 0.5f;
        offsetSize.y = GetComponent<Renderer>().bounds.size.z * 0.5f;
    }

    public void Init(float _msBetweenShots, float _muzzleVel , float _damage, int _numOfMultishot, float _delayBetweenMultipleShots)
    {
        weaponController = GetComponent<WeaponController>();
        Debug.Log("wepons-> " + weaponController);
        foreach (Weapon weapon in weaponController.equippedWeapons)
        {
            weapon.Init(_msBetweenShots, _muzzleVel, _damage, _numOfMultishot, _delayBetweenMultipleShots);
        }
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
        StayInBounds();
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
                    InitBezier(new Vector3(Random.Range(GameManager.Instance.PlayAreaBounds[2] + offsetSize.x, GameManager.Instance.PlayAreaBounds[3] - offsetSize.x), 0,
                                        Random.Range(GameManager.Instance.PlayAreaBounds[0] + offsetSize.y, GameManager.Instance.PlayAreaBounds[1] - offsetSize.y)));
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

    private void StayInBounds()
    {
        if (destination.z > GameManager.Instance?.PlayAreaBounds[0] - offsetSize.y)   //top
        {
            destination = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.PlayAreaBounds[0] - offsetSize.y);
        }
        else if (destination.z < GameManager.Instance?.PlayAreaBounds[1] + offsetSize.y)  //bottom
        {
            destination = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.PlayAreaBounds[1] + offsetSize.y);
        }
        else if (destination.x < GameManager.Instance?.PlayAreaBounds[2] + offsetSize.x) //left
        {
            destination = new Vector3(GameManager.Instance.PlayAreaBounds[2] + offsetSize.x, transform.position.y, transform.position.z);
        }
        else if (destination.x > GameManager.Instance?.PlayAreaBounds[3] - offsetSize.x) //right
        {
            destination = new Vector3(GameManager.Instance.PlayAreaBounds[3] - offsetSize.x, transform.position.y, transform.position.z);
        }
    }

    private void RotateTowardsTarget()
    {
        Quaternion lookRot = Utils.RotateY(GameManager.Instance.playerObj.transform.position, transform.position);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, lookRot, 2.0f * Time.fixedDeltaTime);
    }
}
