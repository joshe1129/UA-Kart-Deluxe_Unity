using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputActionsMenu inputActionMap;
    private Vector2 dir;

    private void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        playerInput = gameObject.GetComponent<PlayerInput>();

        inputActionMap = new InputActionsMenu();
        inputActionMap.PlayerMovement.Movement.Enable();
        inputActionMap.PlayerMovement.Movement.performed += (ctx) => dir = ctx.ReadValue<Vector2>();
        inputActionMap.PlayerMovement.Movement.canceled += (ctx) => dir = Vector2.zero;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Move();
    }
    public void Move ()
    {
        Vector2 inputVector = dir;
        rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
    }
}
