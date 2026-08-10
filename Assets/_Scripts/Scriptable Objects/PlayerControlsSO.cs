using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "PlayerControlsSO", menuName = "Settings Scriptable Objects/PlayerControlsSO")]
public class PlayerControlsSO : ScriptableObject
{
    private bool isInitialized;

    InputAction EscapeAction;
    InputAction MoveAction;
    InputAction lookAction;
    InputAction JumpAction;
    InputAction SprintAction;
    InputAction CrouchAction;
    public InputAction LeftClickAction;
    InputAction RightClickAction;
    public InputAction InteractAction;
    public InputAction ThrowAction;
    InputAction InventoryAction;
    public event Action onEscape;
    public event Action<Vector2> onMove;
    public event Action<Vector2> onLook;
    public event Action Onjump;
    public event Action Onsprint;
    public event Action Oncrouch;
    public event Action Onleftclick;
    public event Action Onrightclick;
    public event Action Oninteract;
    public event Action Onthrow;
    public event Action Oninventory;

    public void Initialize()
    {
        InputSystem.actions.Enable();

        EscapeAction = InputSystem.actions.FindAction("Escape");
        MoveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        JumpAction = InputSystem.actions.FindAction("Jump");
        SprintAction = InputSystem.actions.FindAction("Sprint");
        CrouchAction = InputSystem.actions.FindAction("Crouch");
        LeftClickAction = InputSystem.actions.FindAction("LeftClick");
        RightClickAction = InputSystem.actions.FindAction("RightClick");
        InteractAction = InputSystem.actions.FindAction("Interact");
        ThrowAction = InputSystem.actions.FindAction("Throw");
        InventoryAction = InputSystem.actions.FindAction("Inventory");

        Debug.Log("InventoryAction is: " + (InventoryAction != null ? "found" : "NULL"));

        EscapeAction.performed += ctx => onEscape?.Invoke();
        EscapeAction.canceled += ctx => onEscape?.Invoke();

        MoveAction.performed += ctx => onMove?.Invoke(ctx.ReadValue<Vector2>());
        MoveAction.canceled += ctx => onMove?.Invoke(Vector2.zero);

        lookAction.performed += ctx => onLook?.Invoke(ctx.ReadValue<Vector2>());
        lookAction.canceled += ctx => onLook?.Invoke(Vector2.zero);

        JumpAction.performed += ctx => Onjump?.Invoke();
        JumpAction.canceled += ctx => Onjump?.Invoke();

        SprintAction.performed += ctx => Onsprint?.Invoke();
        SprintAction.canceled += ctx => Onsprint?.Invoke();

        CrouchAction.performed += ctx => Oncrouch?.Invoke();
        CrouchAction.canceled += ctx => Oncrouch?.Invoke();

        LeftClickAction.performed += ctx => Onleftclick?.Invoke();
        LeftClickAction.canceled += ctx => Onleftclick?.Invoke();

        RightClickAction.performed += ctx => Onrightclick?.Invoke();
        RightClickAction.canceled += ctx => Onrightclick?.Invoke();

        InteractAction.performed += ctx => Oninteract?.Invoke();
        InventoryAction.performed += ctx =>
        {
            Debug.Log("Inventory button performed!");
            Oninventory?.Invoke();
        };
        
        ThrowAction.performed += ctx => Onthrow?.Invoke();
        ThrowAction.canceled += ctx => Onthrow?.Invoke();

        InventoryAction.performed += ctx => Oninventory?.Invoke();
        InventoryAction.canceled += ctx => Oninventory?.Invoke();
    }
}