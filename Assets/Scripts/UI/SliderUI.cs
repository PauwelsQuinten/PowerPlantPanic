using UnityEngine;
using UnityEngine.EventSystems;

public class SliderUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameEvent _setSlider;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _setSlider.Raise(this, this.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _setSlider.Raise(this, null);
    }
}
