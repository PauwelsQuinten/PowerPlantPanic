using UnityEngine;

public class InteractUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;
    public void ChangeInteractUI(Component sender, object obj)
    {
        if (_ui.activeSelf)
        {
            _ui.SetActive(false);
        }
        else
        {
            _ui.transform.position = Camera.main.WorldToScreenPoint(sender.transform.position) + (Vector3.up * 5);
            _ui.SetActive(true);

        }
    }
}
