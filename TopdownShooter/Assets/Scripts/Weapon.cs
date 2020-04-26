using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Transform muzzle;
    public Projectile projectile;
    public float msBetweenShots = 100;
    public float muzzleVel = 35;
    float nextShotTime;

    /// <summary>
    /// spawn projectile from objectpool. 
    /// </summary>
    public void Shoot()
    {
        if(Time.time > nextShotTime)
        {
            nextShotTime = Time.time + msBetweenShots / 1000f;
            GameObject objFromPool = ObjectPooler.Instance?.SpawnFromPool("projectile", muzzle.position, muzzle.rotation, null);
            Projectile newProjectile = objFromPool.GetComponent<Projectile>();
            newProjectile.SetSpeed(muzzleVel);
        }
    }
}
