using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public Transform weaponHold;
    public Weapon startingWeapon;
    Weapon equippedWeapon;

    private void Start()
    {
        if (startingWeapon != null)
        {
            EquipWeapon(startingWeapon);
        }
    }

    /// <summary>
    /// Equip weapon at the desired position.
    /// </summary>
    /// <param name="weaponToEquip">weapon obj to equip</param>
    public void EquipWeapon(Weapon weaponToEquip)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon.gameObject);
        }
        equippedWeapon = Instantiate(weaponToEquip, weaponHold.position, weaponHold.rotation) as Weapon;
        equippedWeapon.transform.parent = weaponHold;
    }

    public void Shoot()
    {
        if (equippedWeapon != null)
        {
            equippedWeapon.Shoot();
        }
    }
}
