using UnityEngine;
using UnityEngine.EventSystems;

public class ValveUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameEvent _setValve;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _setValve.Raise(this, this.gameObject.transform.parent.gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _setValve.Raise(this, null);
    }
}
