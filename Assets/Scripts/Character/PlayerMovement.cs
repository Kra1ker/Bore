using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public InputActionAsset InputActions;

    private InputAction IA_moveAction;
    private InputAction IA_lookAction;
    private InputAction IA_jumpAction;

    private Vector2 moveAmount;
    private Vector2 lookAmount;

    public float WalkSpeed = 5;
    public float RotateSpeed = 5;
    public float JumpSpeed = 5;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }
    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        IA_moveAction = InputSystem.actions.FindAction("Move");
        IA_lookAction = InputSystem.actions.FindAction("Look");
        IA_jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveAmount = IA_moveAction.ReadValue<Vector2>();
        lookAmount = IA_lookAction.ReadValue<Vector2>();

        if (IA_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    public void Jump()
    {
        rb.AddForceAtPosition(new Vector2(0, 5f), Vector2.up, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        Walking();
        Rotating();
    }

    private void Walking()
    {
        rb.MovePosition(rb.position + transform.right * moveAmount * WalkSpeed * Time.deltaTime);
    }

    private void Rotating()
    {

    }
}
