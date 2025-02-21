using UnityEngine;
using UnityEngine.EventSystems;

public class HoverScale : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float scale = 1.1f;
    public float initialScale = 1f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        float finalScale = initialScale * scale;
        transform.localScale = Vector3.one * finalScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = Vector3.one * initialScale;
    }
}