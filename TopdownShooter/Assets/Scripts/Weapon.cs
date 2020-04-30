using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public float msBetweenShots = 100;
    public float muzzleVel = 35;
    public float damage = 2f;
    public int numOfMultishot = 2;
    public float delayBetweenMultipleShots = 1.0f;
    float nextShotTime;
    float turnSpeed = 10.0f;
    private Quaternion lookRight;
    private Quaternion lookLeft = Quaternion.identity;
    private bool canSpray = false;
    private Transform root;
    public enum AttackType
    {
        Single,
        Multi,
        Bezier,
        SingleBezier,
        MultiAndBezier
    }
    public AttackType attackType;
    public enum WeaponHolderType
    {
        Player,
        Enemy
    }
    public WeaponHolderType weaponHolderType;
    public enum BezierDir
    {
        None,
        Left,
        Right,
        Forward
    }
    public BezierDir bezierDir;
    public LayerMask projectileCollisionMask;
    public enum ProjectileType
    {
        Red,
        Purple,
        Yellow
    }
    public ProjectileType projectileType;

    private void Awake()
    {
        root = transform.root;
    }

    private void Start()
    {
        lookRight = Quaternion.Euler(0, 45, 0);
    }

    public void Init(float _msBetweenShots, float _muzzleVel, float _damage, int _numOfMultishot, float _delayBetweenMultipleShots)
    {
        msBetweenShots = _msBetweenShots;
        muzzleVel = _muzzleVel;
        damage = _damage;
        numOfMultishot = _numOfMultishot;
        delayBetweenMultipleShots = _delayBetweenMultipleShots;
    }

    /// <summary>
    /// spawn projectile from objectpool. 
    /// </summary>
    public void Shoot()
    {
        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000f;
            if (attackType == AttackType.Single || attackType == AttackType.Bezier || attackType == AttackType.SingleBezier)
            {
                SpawnProjectile();
            }
            else if(attackType == AttackType.Multi || attackType == AttackType.MultiAndBezier)
                StartCoroutine(SpawnProjectileWithDelay(numOfMultishot));
        }
    }

    private void SpawnProjectile()
    {
        string projectileTag = string.Empty;
        if (projectileType == ProjectileType.Red)
            projectileTag = "projectile";
        else if (projectileType == ProjectileType.Purple)
            projectileTag = "purple";
        else
            projectileTag = "yellow";
        Debug.Log("projectileTag-> " + projectileTag);
        GameObject objFromPool = ObjectPooler.Instance?.SpawnFromPool(projectileTag, muzzle.position, transform.rotation, GameManager.Instance.projectileParent);
        Projectile newProjectile = objFromPool.GetComponent<Projectile>();
        newProjectile.Init(projectileCollisionMask, damage);
        if (weaponHolderType == WeaponHolderType.Enemy)
        {
            if(attackType == AttackType.Bezier || attackType == AttackType.MultiAndBezier || attackType == AttackType.SingleBezier)
            {
                Debug.Log("attacktype-> " + attackType);
                //newProjectile.toRotate = root;
                if(bezierDir == BezierDir.Left)
                    newProjectile.InitBezier(GameManager.Instance.playerObj.transform.position, -muzzle.right);
                else if (bezierDir == BezierDir.Right)
                    newProjectile.InitBezier(GameManager.Instance.playerObj.transform.position, muzzle.right);
                else
                    newProjectile.InitBezier(GameManager.Instance.playerObj.transform.position, muzzle.forward);
            }
            else
            {
                //Debug.Log("attacktype-> " + attackType);
                newProjectile.destination = GameManager.Instance.playerObj.transform.position;
            }
        }
        else
        {
            if (attackType == AttackType.Bezier || attackType == AttackType.MultiAndBezier)
            {
                //Debug.Log("attacktype-> " + attackType);
                Vector3 target = new Vector3(0, 0, GameManager.Instance.PlayAreaBounds[0]);
                if(GameManager.Instance.enemyParent.childCount > 0)
                    target = GameManager.Instance.enemyParent.GetChild(Random.Range(0, GameManager.Instance.enemyParent.childCount)).transform.position;
                if (bezierDir == BezierDir.Left)
                    newProjectile.InitBezier(target, -muzzle.right);
                else if (bezierDir == BezierDir.Right)
                    newProjectile.InitBezier(target, muzzle.right);
                else
                    newProjectile.InitBezier(target, muzzle.forward);
            }
        }
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
            yield return new WaitForSeconds(delayBetweenMultipleShots);
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
