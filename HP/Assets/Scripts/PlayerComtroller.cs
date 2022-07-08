using UnityEngine;

enum Position
{
    Left,
    Middle,
    Right
}

public class PlayerComtroller : MonoBehaviour
{
    private CharacterController _characterController;
    private Vector3 _moveDirection;
    private Position _currentPosition = Position.Middle;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private float LerpRate = 0.5f;
    [SerializeField] private float LaneDistance = 2.25f;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _currentPosition = Position.Middle;
    }

    private void Update()
    {
        _moveDirection.z = Speed;
        GetInput();
    }

    private void FixedUpdate()
    {
        _characterController.Move(_moveDirection * Time.fixedDeltaTime);
        ChangePosition();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (_currentPosition != Position.Left)
                _currentPosition--;
        }

        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (_currentPosition != Position.Right)
                _currentPosition++;
        }
    }

    private void ChangePosition()
    {
        Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        if (_currentPosition == Position.Left)
            targetPosition += Vector3.left * LaneDistance;
        else if (_currentPosition == Position.Right)
            targetPosition += Vector3.right * LaneDistance;

        transform.position = Vector3.Lerp(transform.position, targetPosition, LerpRate * Time.fixedDeltaTime);
    }

}
