using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rigidbody2D;

public class MovementController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D _rb;
    [SerializeField]
    private float _movingSpeed = 5, _rotatingSpeed = 1;
    [SerializeField]
    private Animator _bodyAnim, _handsAnim;
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
        if(_moveDirection.ReadValue<Vector2>() == Vector2.zero)
        {
            //Stop the moving animations
            _bodyAnim.StartPlayback();
            _handsAnim.StartPlayback();
            //Don't Move
            return;
        }
        else
        {
            //Play the animation
            _bodyAnim.StopPlayback();
            _handsAnim.StopPlayback();
            //Move the player
            _rb.Slide(_moveDirection.ReadValue<Vector2>(), _movingSpeed * Time.deltaTime, _sMove);
        }

        Vector2 direction = new Vector2(_moveDirection.ReadValue<Vector2>().x, _moveDirection.ReadValue<Vector2>().y);

        if (direction != Vector2.zero)
        {
            float currentAngle = transform.rotation.eulerAngles.z;
            float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            float angle = Mathf.LerpAngle(currentAngle, targetAngle, _rotatingSpeed * Time.deltaTime);
            
            transform.rotation = Quaternion.Euler(0,0, angle);
        }
    }
}
