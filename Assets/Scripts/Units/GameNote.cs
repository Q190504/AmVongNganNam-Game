using System.Collections;
using UnityEngine;

public class GameNote : MonoBehaviour
{
    [SerializeField]
    private float startX;
    private float startY;
    private float endX;
    private float endY;
    private float spawnTime;
    private float travelDuration;
    private HitResult hitResult;
    private SpriteRenderer spriteRenderer;

    public enum HitResult
    {
        NONE,
        PERFECT,
        GOOD,
        BAD,
        MISS
    }

    [SerializeField] GameObjectPublisherSO NoteDisappearPublisher;

    public void Initialize(Vector2 startPoint, Vector2 endPoint, float spawnTime, float travelDuration)
    {
        this.startX = startPoint.x;
        this.startY = startPoint.y;
        this.endX = endPoint.x;
        this.endY = endPoint.y;
        this.spawnTime = spawnTime;
        this.travelDuration = travelDuration;

        this.hitResult = HitResult.NONE;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        Color color = spriteRenderer.color;
        color.a = 1f;
        spriteRenderer.color = color;

        transform.position = new Vector3(startX, startY, 1);
    }

    private float timeDifference;
    public void UpdateNotePosition(float songTime)
    {
        float progress = (songTime - spawnTime) / travelDuration;

        float overshoot = (progress > 1f) ? (1f - progress) * 15f : 0f;

        timeDifference = songTime - (spawnTime + travelDuration);
        if (timeDifference > config.badThreshold)
        {
            hitResult = HitResult.MISS;
            Debug.Log(hitResult.ToString().ToUpper() + "!");
            NoteDisappearPublisher.RaiseEvent(this.gameObject);

        }

        transform.position = new Vector3(Mathf.Lerp(startX, endX + overshoot, progress), Mathf.Lerp(startY, endY, progress), transform.position.z);

    }

    [SerializeField] private ConfigSO config;

    public void CheckHit()
    {
        if (hitResult != HitResult.NONE) return; // Prevent multiple hits

        SetHitResult(timeDifference);

        if (hitResult != HitResult.NONE)
        {
            Debug.Log(hitResult.ToString().ToUpper() + "!"); // Show "PERFECT!", "GOOD!", etc.
            NoteDisappearPublisher.RaiseEvent(this.gameObject);
        }

    }

    private void SetHitResult(float timeDifference)
    {
        if (Mathf.Abs(timeDifference) <= config.perfectThreshold) hitResult = HitResult.PERFECT;
        else if (Mathf.Abs(timeDifference) <= config.goodThreshold) hitResult = HitResult.GOOD;
        else if (Mathf.Abs(timeDifference) <= config.badThreshold) hitResult = HitResult.BAD;
        else hitResult = HitResult.NONE;
    }

    public HitResult GetHitResult()
    {
        return hitResult;
    }

}
