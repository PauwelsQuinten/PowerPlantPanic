using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    private Transform playerTransform;
    [SerializeField]
    private Vector2 _offset = new Vector2(0f,0f);
    [SerializeField]
    private float _smoothTime = 0.3f;

    private Vector2 _playerPosition;
    private Vector3 velocity = Vector3.zero;

    private void LateUpdate()
    {
        _playerPosition = playerTransform.position;

        if (_playerPosition == null) return;

        //Calculate the desired position of the camera
        Vector3 targetPosition = new Vector3(_playerPosition.x + _offset.x, _playerPosition.y + _offset.y, -10);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, _smoothTime);
    }

}
