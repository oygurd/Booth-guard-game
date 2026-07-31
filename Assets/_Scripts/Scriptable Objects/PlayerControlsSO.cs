using UnityEngine;
using UnityEngine.InputSystem;
using System;

[CreateAssetMenu(fileName = "PlayerControlsSO", menuName = "Settings Scriptable Objects/PlayerControlsSO")]
public class PlayerControlsSO : ScriptableObject
{
    InputAction MoveAction;
    InputAction lookAction;
    InputAction JumpAction;
    InputAction SprintAction;
    InputAction CrouchAction;
    InputAction LeftClickAction;
    InputAction RightClickAction;
    InputAction InteractAction;
    
    public event Action<Vector2> onMove;
    public event Action<Vector2> onLook;
    public event Action Onjump;
    public event Action Onsprint;
    public event Action Oncrouch;
    public event Action Onleftclick;
    public event Action Onrightclick;
    public event Action Oninteract;
    
    public void Initialize()
    {
        MoveAction = InputSystem.actions.FindAction("Move");
        JumpAction = InputSystem.actions.FindAction("Jump");
        SprintAction = InputSystem.actions.FindAction("Sprint");
        CrouchAction = InputSystem.actions.FindAction("Crouch");
        LeftClickAction = InputSystem.actions.FindAction("LeftClick");
        RightClickAction = InputSystem.actions.FindAction("RightClick");
        InteractAction = InputSystem.actions.FindAction("Interact");
        
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
        InteractAction.canceled += ctx => Oninteract?.Invoke();

    }
}
