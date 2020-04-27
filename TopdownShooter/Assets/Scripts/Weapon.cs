using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVel = 35;
    public Transform leftTarget, rightTarget;
    float nextShotTime;
    float turnSpeed = 10.0f;
    private Quaternion lookRight;
    private Quaternion lookLeft = Quaternion.identity;
    private bool canSpray = false;
    public enum AttackType
    {
        Single,
        Multi,
        Bezier
    }
    public AttackType attackType;
    public enum WeaponHolderType
    {
        Player,
        Enemy
    }
    public WeaponHolderType weaponHolderType;

    private void Start()
    {
        lookRight = Quaternion.Euler(0, 45, 0);
    }

    /// <summary>
    /// spawn projectile from objectpool. 
    /// </summary>
    public void Shoot()
    {
        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000f;
            if (attackType == AttackType.Single || attackType == AttackType.Bezier)
            {
                SpawnProjectile();
            }
            else if(attackType == AttackType.Multi)
                StartCoroutine(SpawnProjectileWithDelay(5));
        }
    }

    private void SpawnProjectile()
    {
        GameObject objFromPool = ObjectPooler.Instance?.SpawnFromPool("projectile", muzzle.position, muzzle.rotation, GameManager.Instance.projectileParent);
        Projectile newProjectile = objFromPool.GetComponent<Projectile>();
        if (weaponHolderType == WeaponHolderType.Enemy)
        {
            if(attackType == AttackType.Bezier)
            {
                newProjectile.InitBezier(GameManager.Instance.playerObj.transform.position);
            }
            else
                newProjectile.destination = GameManager.Instance.playerObj.transform.position;
        }
        if (attackType == AttackType.Bezier)
            newProjectile.InitBezier(new Vector3(0, 0, 0));
        newProjectile.SetSpeed(muzzleVel);
    }

    private IEnumerator SpawnProjectileWithDelay(int n)
    {
        for (int i = 0; i < n; i++)
        {
            SpawnProjectile();
            if(i == n - 1)
            {
                //Debug.Log("breaking");
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Spray(bool _canSpray)
    {
        canSpray = _canSpray;
    }

    private void FixedUpdate()
    {
        if (canSpray)
            RotateTowardsTarget();
        else
            ResetRotation();
    }

    private void ResetRotation()
    {
        transform.localRotation = Quaternion.identity;
    }

    private void RotateTowardsTarget()
    {
        if (Quaternion.Dot(transform.localRotation, lookRight) <= -0.9999f || Quaternion.Dot(transform.localRotation, lookRight) >= 0.9999f)
        {
            lookLeft = Quaternion.Euler(0, -45, 0);
            lookRight = Quaternion.identity;
        }
        else if (Quaternion.Dot(transform.localRotation, lookLeft) <= -0.9999f || Quaternion.Dot(transform.localRotation, lookLeft) >= 0.9999f)
        {
            lookRight = Quaternion.Euler(0, 45, 0);
            lookLeft = Quaternion.identity;
        }
        if (lookRight != Quaternion.identity)
        {
            //Debug.Log("looking right");
            transform.localRotation = Quaternion.Lerp(transform.localRotation, lookRight, turnSpeed * Time.fixedDeltaTime);
        }
        else if (lookLeft != Quaternion.identity)
        {
            //Debug.Log("looking left");
            transform.localRotation = Quaternion.Lerp(transform.localRotation, lookLeft, turnSpeed * Time.fixedDeltaTime);
        }
    }
}
