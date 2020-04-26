using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(WeaponController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 12.0f;
    private Vector2 startPos;
    private Vector3 moveInput;
    private PlayerController controller;
    private WeaponController weaponController;

    private void Start()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
    }

    private void Update()
    {
        GetInput();
        Vector3 moveVel = moveInput.normalized * moveSpeed;
        controller.Move(moveVel);
        weaponController.Shoot();
    }

    /// <summary>
    /// Get touch input from user.
    /// Get direction from touch input.
    /// </summary>
    private void GetInput()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    Vector2 dir = touch.position - startPos;
                    moveInput = new Vector3(dir.x, 0, dir.y);
                    break;
                case TouchPhase.Stationary:
                    break;
                case TouchPhase.Ended:
                    moveInput = Vector3.zero;   //reset input to stop player from moving.
                    break;
                case TouchPhase.Canceled:
                    break;
                default:
                    break;
            }
        }
    }
}
