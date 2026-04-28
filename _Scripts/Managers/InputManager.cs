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
    public static bool spellLeftWasPressed;
    public static bool spellLeftWasReleased;
    public static bool spellLeftIsHeld;
    public static bool spellRightWasPressed;
    public static bool spellRightWasReleased;
    public static bool spellRightIsHeld;
    public static bool attackWasPressed;
    public static bool attackWasReleased;
    public static bool attackIsHeld;
    public static bool jumpWasPressed;
    public static bool jumpIsHeld;
    public static bool jumpWasReleased;
    public static bool runIsHeld;
    public static bool DialogSkipPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _spellLeftAction;
    private InputAction _spellRightAction;
    private InputAction _DialogSkipAction;
    private InputAction _attackAction;

    private void Awake()
    {
        PlayerInput = GetComponent<PlayerInput>();

        _moveAction = PlayerInput.actions["Move"];
        _jumpAction = PlayerInput.actions["Jump"];
        _runAction = PlayerInput.actions["Run"];
        _spellLeftAction = PlayerInput.actions["SpellLeft"];
        _spellRightAction = PlayerInput.actions["SpellRight"];
        _attackAction = PlayerInput.actions["Attack"];
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

        // spell actions
        spellLeftWasPressed = _spellLeftAction.WasPressedThisFrame();
        spellLeftWasReleased = _spellLeftAction.WasReleasedThisFrame();
        spellLeftIsHeld = _spellLeftAction.IsPressed();

        spellRightWasPressed = _spellRightAction.WasPressedThisFrame();
        spellRightWasReleased = _spellRightAction.WasReleasedThisFrame();
        spellRightIsHeld = _spellRightAction.IsPressed();

        // attack actions
        attackWasPressed = _attackAction.WasPressedThisFrame();
        attackWasReleased = _attackAction.WasReleasedThisFrame();
        attackIsHeld = _attackAction.IsPressed();

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
