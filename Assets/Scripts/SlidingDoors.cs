using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SlidingDoors : MonoBehaviour
{
    [SerializeField]
    private GameObject doorLeft, doorRight;
    [SerializeField]
    private Transform doorLeftPosition, doorRightPosition;
    [SerializeField]
    private float doorSpeed = 1;

    private Vector2 _leftClosedPosition, _rightOpenPosition;

    private void Start()
    {
        _leftClosedPosition = doorLeft.transform.position;
        _rightOpenPosition = doorRight.transform.position;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer != 3f) return;

        StopAllCoroutines();
        StartCoroutine(OpenDoors());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer != 3f) return;

        StopAllCoroutines();
        StartCoroutine(CloseDoors());
    }

    IEnumerator OpenDoors() 
    {
        Vector2 leftDoorTargetPos = doorLeftPosition.position;
        Vector2 currentLeftDoorPos = doorLeft.transform.position;

        while(Vector2.Distance(currentLeftDoorPos, leftDoorTargetPos) > 0.01f)
        {
            doorLeft.transform.position = Vector2.MoveTowards(doorLeft.transform.position, leftDoorTargetPos, doorSpeed * Time.deltaTime);
            doorRight.transform.position = Vector2.MoveTowards(doorRight.transform.position, doorRightPosition.position, doorSpeed * Time.deltaTime);

            yield return null;
        }

        doorLeft.transform.position = doorLeftPosition.position;
        doorRight.transform.position = doorRightPosition.position;
    }

    IEnumerator CloseDoors()
    {
        Vector2 leftDoorTargetPos = _leftClosedPosition;
        Vector2 currentLeftDoorPos = doorLeft.transform.position;

        while (Vector2.Distance(currentLeftDoorPos, leftDoorTargetPos) > 0.01f)
        {
            doorLeft.transform.position = Vector2.MoveTowards(doorLeft.transform.position, _leftClosedPosition, doorSpeed * Time.deltaTime);
            doorRight.transform.position = Vector2.MoveTowards(doorRight.transform.position, _rightOpenPosition, doorSpeed * Time.deltaTime);

            yield return null;
        }

        doorLeft.transform.position = _leftClosedPosition;
        doorRight.transform.position = _rightOpenPosition;
    }
}
