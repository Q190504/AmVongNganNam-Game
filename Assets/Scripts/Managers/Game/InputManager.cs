using System.Collections.Generic;
using TheHeroesJourney;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] VoidPublisherSO noteTapPublisher;
    [SerializeField] VoidPublisherSO togglePauseGameSO;
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
            PauseManager.Instance.TogglePause();
            togglePauseGameSO.RaiseEvent();
        }
    }
}
