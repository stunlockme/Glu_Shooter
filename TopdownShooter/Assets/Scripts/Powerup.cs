using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    float speed = 5f;
    public float healAmount = 20f;
    public enum PowerupType
    {
        WeaponSpray,
        Health
    }
    public PowerupType powerupType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.attachedRigidbody.tag);
        if(other.attachedRigidbody.tag == "Player")
        {
            Player player = other.attachedRigidbody.GetComponent<Player>();
            if (powerupType == PowerupType.WeaponSpray)
                player.sprayPowerup = true;
            else
                player.healthBar.CurrentValue += healAmount; 
            gameObject.SetActive(false);
            gameObject.transform.parent = ObjectPooler.Instance.PoolParent;
        }
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
}
