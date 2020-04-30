using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public GameObject sideGunObj;
    Rigidbody myRigidbody;
    Vector3 velocity;
    private Renderer myRenderer;
    private Vector2 offsetSize;
    float sideGunOffsetSize;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.useGravity = false;
        myRenderer = GetComponent<Renderer>();
        sideGunOffsetSize = sideGunObj.GetComponent<Renderer>().bounds.size.x + 1.0f;
        offsetSize.x = (myRenderer.bounds.size.x * 0.5f);
        offsetSize.y = myRenderer.bounds.size.z * 0.5f;
    }

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    private void Update()
    {
        StayInBounds();
    }

    /// <summary>
    /// Reset position to be within playarea if this object's position exceeds bounds.
    /// </summary>
    private void StayInBounds()
    {
        if (transform.position.z > GameManager.Instance?.PlayAreaBounds[0] - offsetSize.y)   //top
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.PlayAreaBounds[0] - offsetSize.y);
        }
        else if (transform.position.z < GameManager.Instance?.PlayAreaBounds[1] + offsetSize.y)  //bottom
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, GameManager.Instance.PlayAreaBounds[1] + offsetSize.y);
        }
        else if (transform.position.x < GameManager.Instance?.PlayAreaBounds[2] + offsetSize.x) //left
        {
            transform.position = new Vector3(GameManager.Instance.PlayAreaBounds[2] + offsetSize.x, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > GameManager.Instance?.PlayAreaBounds[3] - (offsetSize.x + sideGunOffsetSize)) //right
        {
            transform.position = new Vector3(GameManager.Instance.PlayAreaBounds[3] - (offsetSize.x + sideGunOffsetSize), transform.position.y, transform.position.z);
        }
    }

    public void FixedUpdate()
    {
        myRigidbody.MovePosition(myRigidbody.position + velocity * Time.fixedDeltaTime);
    }
}
