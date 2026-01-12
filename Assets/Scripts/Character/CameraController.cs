using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{
    private CinemachinePositionComposer CM_PComposer;
    public InputActionAsset InputActions;
    private InputAction IA_lookAction;
    private Vector2 _lookAmount;
    [Header("Parameters")]public float RotateSpeed = 0.1f;
    public float CameraOffset = 0.3f;
    public float StandardOffset = 0.19f;
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
        _lookAmount = IA_lookAction.ReadValue<Vector2>();
        Shift();
    }

    private void Shift()
    {
        _lookAmount.y = Mathf.Clamp(_lookAmount.y, -lookClamp, lookClamp);

        if (_lookAmount.y > 0 && CameraOffset * _lookAmount.y >= StandardOffset)
        {
            _timeLookReleased = 0f;
            CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, CameraOffset * _lookAmount.y, RotateSpeed);
        }
        else if (_lookAmount.y < 0 && CameraOffset * _lookAmount.y <= StandardOffset)
        {
            _timeLookReleased = 0f;
            CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, CameraOffset * _lookAmount.y, RotateSpeed);
        }
        else
        {
            _timeLookReleased += Time.deltaTime;
            if (_timeLookReleased > returnDelay)
                CM_PComposer.Composition.ScreenPosition.y = Mathf.Lerp(CM_PComposer.Composition.ScreenPosition.y, StandardOffset, RotateSpeed);
        }
    }
}
