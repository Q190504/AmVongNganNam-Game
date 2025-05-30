using UnityEngine;
using UnityEngine.UI;

public class RawImageScroller : MonoBehaviour
{
    public RawImage rawImage;
    public Vector2 scrollSpeed = new Vector2(0.0f, 0.0f);

    private Rect uvRect;

    void Start()
    {
        uvRect = rawImage.uvRect;
    }

    void Update()
    {
        uvRect.x += scrollSpeed.x * Time.deltaTime;
        uvRect.y += scrollSpeed.y * Time.deltaTime;

        rawImage.uvRect = uvRect;
    }
}
