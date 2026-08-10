using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class FpsCamController : MonoBehaviour
{
    public static FpsCamController instance;
    [SerializeField] float masterSensitivity;
    [SerializeField] private float xMinClamp, xMaxClamp;

    private InputAction MouseDelta;

    public bool updatingRotation;
    
    private Vector2 mouseRotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this;
        MouseDelta = InputSystem.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!updatingRotation) return;
        
        Vector2 mouseDelta = MouseDelta.ReadValue<Vector2>();
        mouseRotation.x += -mouseDelta.y * masterSensitivity / 20;
        mouseRotation.y += mouseDelta.x * masterSensitivity  / 20;

        mouseRotation.x = Mathf.Clamp(mouseRotation.x, xMinClamp, xMaxClamp);
        transform.localRotation = Quaternion.Euler(mouseRotation);

    }

    /*private void OnLook(InputAction.CallbackContext context)
    {
        Vector2 mouseDelta = context.ReadValue<Vector2>();
        
        mouseRotation.x = mouseDelta.y;
        mouseRotation.y = mouseDelta.x;
    }*/
    private void FixedUpdate()
    {
       

    }
}
