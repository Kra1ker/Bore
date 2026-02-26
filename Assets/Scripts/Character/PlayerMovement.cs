using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace BorePlayerMovement
{

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Settings")]
        private float _time;
        [SerializeField] private float _frameLeftGrounded = float.MinValue;
        private Rigidbody2D _rb;
        private BoxCollider2D _col;
        [SerializeField] private LayerMask layerMask;
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _attackAction;
        private InputAction _exitAction;
        private InputAction _restartAction;
        private Vector2 _moveAmount;

        [Header("Parameters")]
        public float WalkSpeed = 5;
        public float JumpSpeed = 20;

        [Header("State")]
        private bool isGrounded;
        private bool _grounded;
        [SerializeField] private float rayLenght = 0.02f;
        public bool СoyoteUsable;
        public float CoyoteTime = 0.15f;
        public bool canUseCoyote => СoyoteUsable && !isGrounded && _time < _frameLeftGrounded + CoyoteTime;

        #region Initialization

        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
            _moveAction = _playerInput.actions["Move"];
            _jumpAction = _playerInput.actions["Jump"];
            _attackAction = _playerInput.actions["Attack"];
            _exitAction = _playerInput.actions["Exit"];
            _restartAction = _playerInput.actions["Restart"];
            

            _rb = GetComponent<Rigidbody2D>();
            _col = GetComponent<BoxCollider2D>();
            layerMask = LayerMask.GetMask("Ground");
        }
        #endregion

        #region PlayerState
        private void Update()
        {
            _moveAmount = _moveAction.ReadValue<Vector2>();

            if (_jumpAction.WasPressedThisFrame() && (isGrounded || canUseCoyote))
            {
                Jump();
            }
            if(_attackAction.WasPressedThisFrame())
            {
                Attack();
            }
            if(_restartAction.WasPressedThisFrame())
            {
                Restart();
            }
            if(_exitAction.WasPressedThisFrame())
            {
                Exit();
            }
        }

        void FixedUpdate()
        {
            Walking();
            CheckGrounded();
            _time += Time.fixedDeltaTime;
        }

        private void Walking()
        {
            _rb.linearVelocityX = _moveAmount.x * WalkSpeed;
        }

        public void Jump()
        {
            _rb.AddForceY(JumpSpeed, ForceMode2D.Impulse);
            СoyoteUsable = false;
            _grounded = false;
            isGrounded = false;
        }

        public void Attack()
        {
            Debug.Log(this + " attacked.");
            Vector2 cOrigin = _col.bounds.center - new Vector3(_col.bounds.extents.x, 0, 0);
            Debug.DrawRay(cOrigin, Vector2.left * 1, Color.red);
        }
        #endregion

        public void Restart()
        {
            Scene current = SceneManager.GetActiveScene();
            SceneManager.LoadScene(current.buildIndex);
        }
        
        public void Exit()
        {
            Application.Quit();

            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #endif
        }

        #region Colision
        private void CheckGrounded()
        {
            // c - Center | l - Left | r - Right | rc - RayCast
            Vector2 cOrigin = _col.bounds.center - new Vector3(0, _col.bounds.extents.y, 0);
            Vector2 lOrigin = _col.bounds.center - new Vector3(_col.bounds.extents.x, _col.bounds.extents.y, 0);
            Vector2 rOrigin = _col.bounds.center + new Vector3(_col.bounds.extents.x, -_col.bounds.extents.y, 0);

            RaycastHit2D cRc = Physics2D.Raycast(cOrigin, Vector2.down, rayLenght, layerMask);
            RaycastHit2D lRc = Physics2D.Raycast(lOrigin, Vector2.down, rayLenght, layerMask);
            RaycastHit2D rRc = Physics2D.Raycast(rOrigin, Vector2.down, rayLenght, layerMask);
            isGrounded = cRc.collider != null || lRc.collider != null || rRc.collider != null;

            // Checking is NOW character grounded
            if (!_grounded && isGrounded)
            {
                _grounded = true;
                СoyoteUsable = true;
            }
            else if (_grounded && !isGrounded)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
            }

            // ** DEBUG **
            Debug.DrawRay(cOrigin, Vector2.down * rayLenght, Color.red);
            Debug.DrawRay(lOrigin, Vector2.down * rayLenght, Color.red);
            Debug.DrawRay(rOrigin, Vector2.down * rayLenght, Color.red);
        }
        #endregion
    }
}