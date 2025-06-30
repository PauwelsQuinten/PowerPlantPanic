using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PowerRegulator : MonoBehaviour, IMiniGame
{
    [SerializeField]
    private GameObject _miniGameUI;
    [SerializeField]
    private int _numberOfSliders;
    [SerializeField]
    List<GameObject> _sliderObject = new List<GameObject>();
    [SerializeField]
    List<GameObject> _disiredLocations = new List<GameObject>();

    private List<int> _yStartPoints = new List<int>();
    private List<int> _yfinishPoints = new List<int>();

    private Vector2 _mousePos;

    private GameObject _activeSlider;

    private bool _isHoldingSlider;
    public void StartMiniGame(Component sender, object obj)
    {
        initializeMiniGame();
    }
    public void completed()
    {
        throw new System.NotImplementedException();
    }

    public void failed()
    {
        throw new System.NotImplementedException();
    }

    private void initializeMiniGame()
    {
        for (int i = 0; i < _numberOfSliders; i++)
        {
            int startHeight = Random.Range(1, 6);
            int desiredHeight = Random.Range(1, 6);
            while(startHeight >= desiredHeight - 1 && startHeight <= desiredHeight + 1)
            {
                 startHeight = Random.Range(1, 6);
            }
            _yStartPoints.Add(-260 + ((startHeight - 1) * 130));

            _yfinishPoints.Add(-260 + ((desiredHeight - 1) * 130));
        }

        for(int i = 0; i < _yStartPoints.Count; i++)
        {
            Vector3 newPos = _sliderObject[i].transform.localPosition;
            newPos.y = _yStartPoints[i];
            _sliderObject[i].transform.localPosition = newPos;
            newPos.y = _yfinishPoints[i];
            _disiredLocations[i].transform.localPosition = newPos;
        }

        _miniGameUI.SetActive(true);
    }

    private void Update()
    {
        if (!_miniGameUI.activeSelf) return;
        if (Mouse.current.leftButton.IsPressed())
        {
            _isHoldingSlider = true;
            if (_activeSlider == null) return;

            Vector2 objectPos = _activeSlider.transform.position;
            Vector2 mousePos = Mouse.current.position.ReadValue();
            if (Vector2.Distance(objectPos, mousePos) < 130) return;
            Debug.Log("reachedMax");
            if (objectPos.y < mousePos.y && objectPos.y < 260)
            {
                objectPos.y += 130;
            }
            else if (objectPos.y > mousePos.y && objectPos.y > -260)
            {
                objectPos.y -= 130;
            }

            _activeSlider.transform.position = objectPos;

        }
        else if(Mouse.current.leftButton.wasReleasedThisFrame)
        {
            _isHoldingSlider = false;
            _activeSlider = null;
        }
    }

        public void SetActivatedSlider(Component sender, object obj)
    {
        if (_isHoldingSlider) return;
        _activeSlider = obj as GameObject;
    }
}
