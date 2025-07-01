using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;

    private GameObject _currentUICaller;
    public void ChangeInteractUI(Component sender, object obj)
    {
        bool? setActive = obj as bool?;

        if (!(bool)setActive)
        {
            _currentUICaller = null;
            _ui.SetActive(false);
        }
        else
        {
            _currentUICaller = sender.gameObject;
            _ui.SetActive(true);
        }
    }

    private void Update()
    {
        if(_currentUICaller == null) return;
        _ui.transform.position = Camera.main.WorldToScreenPoint(_currentUICaller.transform.position) + (Vector3.up * 5);
    }
}
