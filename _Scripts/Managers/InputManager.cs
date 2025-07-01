using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static PlayerInput PlayerInput;

    public static Vector2 Movement;
    private static float _previousVerticalInput;
    private static float _previousHorizontalInput;
    public static bool moveUpWasReleased;
    public static bool moveDownWasReleased;
    public static bool moveLeftWasReleased;
    public static bool moveRightWasReleased;
    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpWasReleased;
    public static bool runIsHeld;
    public static bool DialogSkipPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _DialogSkipAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        _DialogSkipAction = PlayerInput.actions["DialogSkip"];

    }

    private void Update()
    {
        // movement actions
        Movement = _moveAction.ReadValue<Vector2>();
        moveUpWasReleased = _previousVerticalInput > 0 && Movement.y == 0;
        moveDownWasReleased = _previousVerticalInput < 0 && Movement.y == 0;
        moveLeftWasReleased = _previousHorizontalInput < 0 && Movement.y == 0;
        moveRightWasReleased = _previousHorizontalInput > 0 && Movement.y == 0;

        _previousHorizontalInput = Movement.x;
        _previousVerticalInput = Movement.y;


        // jump actions
        jumpWasPressed = _jumpAction.WasPressedThisFrame();
        jumpIsHeld = _jumpAction.IsPressed();
        jumpWasReleased = _jumpAction.WasReleasedThisFrame();

        // run action
        runIsHeld = _runAction.IsPressed();

        // dialog action
        DialogSkipPressed = _DialogSkipAction.WasPressedThisFrame();
    }
}
