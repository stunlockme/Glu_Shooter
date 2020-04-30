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

    /// <summary>
    /// player powerup assigned using powerupType.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.attachedRigidbody.tag);
        if(other.attachedRigidbody.tag == "Player")
        {
            Player player = other.attachedRigidbody.GetComponent<Player>();
            if (powerupType == PowerupType.WeaponSpray)
                player.sprayPowerup = true;
            else
            {
                player.health += healAmount;
                player.healthBar.CurrentValue += healAmount;
            }
            gameObject.SetActive(false);
            gameObject.transform.parent = ObjectPooler.Instance.PoolParent;
        }
    }

    private void Update()
    {
        DisableObj();
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    /// <summary>
    /// Disable object if outside playarea.
    /// </summary>
    private void DisableObj()
    {
        if (transform.position.z < GameManager.Instance?.PlayAreaBounds[1])
        {
            gameObject.SetActive(false);
            gameObject.transform.parent = ObjectPooler.Instance?.PoolParent.transform;
        }
    }
}
