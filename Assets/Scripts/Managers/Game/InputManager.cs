using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Sequence = DG.Tweening.Sequence;

public class InputManager : MonoBehaviour
{
    [SerializeField] VoidPublisherSO noteTapPublisher;
    [SerializeField] VoidPublisherSO togglePauseGameSO;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform endPos;
    [SerializeField] private Vector3 playerInitPos;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private float moveDuration = 0.25f;
    [SerializeField] private float returnDelay = 0.25f;
    [SerializeField] private string attackTriggerName = "Attack";

    private void Start()
    {
        if (player != null)
            playerInitPos = new Vector3 (player.transform.position.x, player.transform.position.y);
    }
    public void Tap(InputAction.CallbackContext context)
    {
        if (PauseManager.IsPause) return;

        if(context.performed)
        {
            noteTapPublisher.RaiseEvent();
            if (playerAnimator != null)
                playerAnimator.SetTrigger(attackTriggerName);
            Debug.Log("Note tapped");
        }
    }

    private Tween moveTween;

    public void Hit(GameObject note)
    {
        if (note == null || player == null) return;

        GameNote gameNote = note.GetComponent<GameNote>();
        if (gameNote == null) return;

        if (gameNote.GetHitResult() == GameNote.HitResult.MISS || gameNote.GetHitResult() == GameNote.HitResult.NONE) return;

        Vector3 notePos = note.transform.position;
        Vector3 targetPos = new Vector3(endPos.position.x, notePos.y, playerInitPos.z);

        moveTween?.Kill();

        Sequence attackSequence = DOTween.Sequence();

        attackSequence.Append(player.transform.DOMove(targetPos, moveDuration).SetEase(Ease.OutQuad));

        attackSequence.AppendInterval(returnDelay);

        attackSequence.Append(player.transform.DOMove(playerInitPos, moveDuration).SetEase(Ease.InQuad));

        moveTween = attackSequence;
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
