using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "PlayerControlsSO", menuName = "Settings Scriptable Objects/PlayerControlsSO")]
public class PlayerControlsSO : ScriptableObject
{
    InputAction MoveAction;
    InputAction JumpAction;
    InputAction SprintAction;
    InputAction CrouchAction;
}
