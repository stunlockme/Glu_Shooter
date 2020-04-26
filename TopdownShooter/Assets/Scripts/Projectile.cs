using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    float speed = 10;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    private void Update()
    {
        AddBackToPool();
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
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
