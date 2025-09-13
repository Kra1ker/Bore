using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    private Rigidbody2D rb;
    public InputActionAsset InputActions;
    private InputAction IA_moveAction;

    private InputAction IA_jumpAction;

    private Vector2 moveAmount;

    [Header("Parameters")]
    public float WalkSpeed = 5;
    public float JumpSpeed = 5;
    private bool isGrounded;

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
        IA_jumpAction = InputSystem.actions.FindAction("Jump");

        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        moveAmount = IA_moveAction.ReadValue<Vector2>();

        if (IA_jumpAction.WasPressedThisFrame())
        {
            Jump();
        }
    }

    public void Jump()
    {
        rb.AddForceY(JumpSpeed, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        Walking();
    }

    private void Walking()
    {
        rb.linearVelocityX = moveAmount.x * WalkSpeed;
    }
}
