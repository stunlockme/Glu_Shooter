using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform[] weaponSpawns;
    public Weapon startingWeapon;
    public Weapon[] equippedWeapons;

    private void Start()
    {
        //equippedWeapons = new Weapon[weaponSpawns.Length];
        //for (int i = 0; i < weaponSpawns.Length; i++)
        //{
        //    EquipWeapon(startingWeapon, weaponSpawns[i], i);
        //}
    }

    /// <summary>
    /// Equip weapon at the desired position.
    /// </summary>
    /// <param name="weaponToEquip">weapon obj to equip</param>
    public void EquipWeapon(Weapon weaponToEquip, Transform weaponSpawnPoint, int i)
    {
        equippedWeapons[i] = ObjectPooler.Instance.SpawnFromPool("weapon", weaponSpawnPoint.position, weaponSpawnPoint.rotation,
                                                                weaponSpawnPoint).GetComponent<Weapon>();
    }

    public void Shoot(bool canSpray)
    {
        foreach (Weapon weapon in equippedWeapons)
        {
            weapon.Spray(canSpray);
            weapon.Shoot();
        }
    }
}
