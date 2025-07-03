using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDrag : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private bool _onObject, _clicked, _enteredGarbage;
    private float _timer;

    private void Update()
    {
        if(_onObject == true &&  _clicked == true)
        {
            transform.position = Input.mousePosition;
        }

        if (_enteredGarbage)
        {
            _timer += Time.deltaTime;

            if (_timer > 1.5f)
            {
               this.gameObject.GetComponent<Image>().enabled = false;
                _timer = 0;
                _enteredGarbage = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onObject = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clicked = !_clicked;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        _enteredGarbage = true;
    }
}
