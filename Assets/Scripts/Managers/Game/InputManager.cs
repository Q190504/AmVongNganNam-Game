using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] VoidPublisherSO noteTapPublisher;
    [SerializeField] VoidPublisherSO pauseGameSO;
    public void Tap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            noteTapPublisher.RaiseEvent();
            Debug.Log("Note tapped");
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            pauseGameSO.RaiseEvent();
        }
    }
}
