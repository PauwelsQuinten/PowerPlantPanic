using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rigidbody2D;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _speed = 1;
    [SerializeField] 
    private InputAction _moveDirection;

    private SlideMovement _sMove;

    private void OnEnable()
    {
        _moveDirection.Enable();
    }

    private void OnDisable()
    {
        _moveDirection.Disable();
    }

    private void Start()
    {
        _sMove.startPosition = new Vector2(0,0);
        _sMove.maxIterations = 20;
    }

    private void Update()
    {
        if(_moveDirection.ReadValue<Vector2>() != Vector2.zero)
        {
            _rb.Slide(_moveDirection.ReadValue<Vector2>(), _speed * Time.deltaTime, _sMove);
        }
    }
}
