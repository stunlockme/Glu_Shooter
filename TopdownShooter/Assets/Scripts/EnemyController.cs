using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Vector3 velocity;

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

}
