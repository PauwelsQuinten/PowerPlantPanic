using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseDrag : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    private bool _onObject, _clicked, _enteredGarbage;
    private float _timer;

    private void Update()
    {
        if(_onObject && _clicked)
        {
            transform.position = Input.mousePosition;
        }

        if (_enteredGarbage && !_clicked)
        {
            _timer += Time.deltaTime;

            if (_timer > 1.5f)
            {
                _timer = 0;
                _enteredGarbage = false;
                this.gameObject.GetComponent<Image>().enabled = false;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _onObject = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_onObject) return;
        _clicked = !_clicked;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        _enteredGarbage = true;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        _enteredGarbage = false;
    }

    private void OnEnable()
    {
        _timer = 0;
        _enteredGarbage = false;
        _onObject = false;
        _clicked = false;
    }
}
