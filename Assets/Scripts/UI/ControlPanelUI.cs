using TMPro;
using UnityEngine;

public class ControlPanelUI : MonoBehaviour
{
    [SerializeField]
    private GameObject _ui;
    [SerializeField]
    private TextMeshProUGUI _powerEfficiency;

    public void EnableUi(Component sender, object obj)
    {
        bool? setActive = obj as bool?;
        if((bool)setActive) _ui.SetActive(true);
        else _ui.SetActive(false);
    }

    public void PowerEfficiencyChanged(Component sender, object obj)
    {
        PowerEfficiencyChangedEventArgs args = obj as PowerEfficiencyChangedEventArgs;
        if (args == null) return;

        _powerEfficiency.text = $"{args.PowerEfficiency} %";
    }
}
