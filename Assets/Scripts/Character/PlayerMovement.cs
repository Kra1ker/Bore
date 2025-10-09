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
        [SerializeField] private float _time;
        [SerializeField] private float _frameLeftGrounded;
        private Rigidbody2D rb;
        private BoxCollider2D col;
        [SerializeField] private LayerMask layerMask;
        public InputActionAsset InputActions;
        private InputAction IA_moveAction;
        private InputAction IA_jumpAction;
        private Vector2 moveAmount;
        private RaycastHit2D raycastHit2D;

        [Header("Parameters")]
        public float WalkSpeed = 5;
        public float JumpSpeed = 20;
        private bool isGrounded;
        [SerializeField] private float rayLenght = 0.02f;
        public bool coyoteEnable;
        public float CoyoteTime = 1.5f;
        public bool canUseCoyote => coyoteEnable && !isGrounded && _time < _frameLeftGrounded + CoyoteTime;

        #region Initialization
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
        #endregion

        #region PlayerState
        private void Update()
        {
            moveAmount = IA_moveAction.ReadValue<Vector2>();

            if (IA_jumpAction.WasPressedThisFrame() && (isGrounded || canUseCoyote))
            {
                Jump();
            }
        }

        void FixedUpdate()
        {
            Walking();
            CheckGrounded();
            _time += Time.deltaTime;
            if (isGrounded != true)
            {
                _frameLeftGrounded = _time;
            }
        }

        private void Walking()
        {
            rb.linearVelocityX = moveAmount.x * WalkSpeed;
        }


        public void Jump()
        {
            rb.AddForceY(JumpSpeed, ForceMode2D.Impulse);
        }
        #endregion

        #region Colision
        private void CheckGrounded()
        {
            // c - Center | l - Left | r - Right | rc - RayCast
            Vector2 cOrigin = col.bounds.center - new Vector3(0, col.bounds.extents.y, 0);
            Vector2 lOrigin = col.bounds.center - new Vector3(col.bounds.extents.x, col.bounds.extents.y, 0);
            Vector2 rOrigin = col.bounds.center + new Vector3(col.bounds.extents.x, -col.bounds.extents.y, 0);

            RaycastHit2D cRc = Physics2D.Raycast(cOrigin, Vector2.down, rayLenght, layerMask);
            RaycastHit2D lRc = Physics2D.Raycast(lOrigin, Vector2.down, rayLenght, layerMask);
            RaycastHit2D rRc = Physics2D.Raycast(rOrigin, Vector2.down, rayLenght, layerMask);
            isGrounded = cRc.collider != null || lRc.collider != null || rRc.collider != null;

            // ** DEBUG **
            Debug.DrawRay(cOrigin, Vector2.down * rayLenght, Color.red);
            Debug.DrawRay(lOrigin, Vector2.down * rayLenght, Color.red);
            Debug.DrawRay(rOrigin, Vector2.down * rayLenght, Color.red);
        }
        #endregion
    }
}