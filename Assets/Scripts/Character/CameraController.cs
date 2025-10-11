using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private CinemachinePositionComposer CM_PComposer;
    public InputActionAsset InputActions;
    private InputAction IA_lookAction;
    private Vector2 lookAmount;
    [Header("Parameters")]public float RotateSpeed = 0.1f;
    public float cameraOffset = 0.3f;
    public float standardOffset = 0.19f;
    private float _timeLookReleased = 0f;
    [SerializeField] private float returnDelay = 0.5f;
    [SerializeField] private float lookClamp = 1f;

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
        IA_lookAction = InputSystem.actions.FindAction("Look");
        CM_PComposer = GetComponent<CinemachinePositionComposer>();
    }
    private void Update()
    {
        lookAmount = IA_lookAction.ReadValue<Vector2>();
        Shift();
    }

    private void Shift()
    {
        lookAmount.y = Mathf.Clamp(lookAmount.y, -lookClamp, lookClamp);

        if (lookAmount.y > 0 && cameraOffset * lookAmount.y >= standardOffset) // Mathf.Abs(lookAmount.y) > 0.05f
        {
            _timeLookReleased = 0f;
            CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, cameraOffset * lookAmount.y, RotateSpeed);
        }
        else if (lookAmount.y < 0 && cameraOffset * lookAmount.y <= standardOffset) // Mathf.Abs(lookAmount.y) < 0.05f
        {
            _timeLookReleased = 0f;
            CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, cameraOffset * lookAmount.y, RotateSpeed);
        }
        else
        {
            _timeLookReleased += Time.deltaTime;
            if (_timeLookReleased > returnDelay)
                CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, standardOffset, RotateSpeed);
        }
    }
}
