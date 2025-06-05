using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class URLClickHandler : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected string url;

    public void OnPointerClick(PointerEventData eventData)
    {
        Application.OpenURL(url);
    }
}
