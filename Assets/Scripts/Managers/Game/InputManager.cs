using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] VoidPublisherSO noteTapPublisher;
    public void Tap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            noteTapPublisher.RaiseEvent();
            Debug.Log("Note tapped");
        }
    }
}
