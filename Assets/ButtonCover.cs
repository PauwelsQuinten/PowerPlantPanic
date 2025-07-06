using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonCover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private GameObject _buttoncoverClosed;
    [SerializeField]
    private GameObject _buttoncoverOpen;
    public void OnPointerEnter(PointerEventData eventData)
    {
        _buttoncoverClosed.SetActive(false);
        _buttoncoverOpen.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttoncoverClosed.SetActive(true);
        _buttoncoverOpen.SetActive(false);
    }
    private void Start()
    {
        _buttoncoverClosed.SetActive(true);
        _buttoncoverOpen.SetActive(false);
    }

}
