using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BorePlayerMovement
{

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        private Rigidbody2D rb;
        private BoxCollider2D col;
        private LayerMask layerMask;

        public InputActionAsset InputActions;
        private InputAction IA_moveAction;
        private InputAction IA_jumpAction;
        private Vector2 moveAmount;
        private RaycastHit2D raycastHit2D;

        [Header("Parameters")]
        public float WalkSpeed = 5;
        public float JumpSpeed = 5;
        private bool isGrounded;
        [SerializeField] private float _time;

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
            col = GetComponent<BoxCollider2D>();
            layerMask = LayerMask.GetMask("Ground");
        }

        private void Update()
        {
            moveAmount = IA_moveAction.ReadValue<Vector2>();

            if (IA_jumpAction.WasPressedThisFrame() && isGrounded)
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
            CheckGrounded();
            _time += Time.deltaTime;
        }

        private void Walking()
        {
            rb.linearVelocityX = moveAmount.x * WalkSpeed;
        }

        private void CheckGrounded()
        {
            RaycastHit2D rc = Physics2D.Raycast(col.bounds.center, Vector2.down, 2, layerMask);
            isGrounded = rc.collider != null;
            Debug.DrawRay(col.bounds.center, Vector2.down * 2, Color.red, 2);
        }
    }
}