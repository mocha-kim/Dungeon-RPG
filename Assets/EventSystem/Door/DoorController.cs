using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorType
{
    DoorL,
    DoorR,
    Portcullis
}

public class DoorController : MonoBehaviour
{
    public DoorEventObject doorEventObject;

    public int id = 0;
    public float openOffset = 100f;
    public float closeOffset = 0f;

    public DoorType type = DoorType.DoorL;

    private void OnEnable()
    {
        doorEventObject.OnOpenDoor += OnOpenDoor;
        doorEventObject.OnCloseDoor += OnCloseDoor;
    }

    private void OnDisable()
    {
        doorEventObject.OnOpenDoor -= OnOpenDoor;
        doorEventObject.OnCloseDoor -= OnCloseDoor;
    }

    public void OnOpenDoor(int id)
    {
        if (id != this.id)
            return;

        StopAllCoroutines();
        switch (type)
        {
            case DoorType.DoorR:
                StartCoroutine(OpenDoorR());
                break;
            case DoorType.DoorL:
                StartCoroutine(OpenDoorL());
                break;
            case DoorType.Portcullis:
                StartCoroutine(OpenPortcullis());
                break;
        }
    }

    public void OnCloseDoor(int id)
    {
        if (id != this.id)
            return;

        StopAllCoroutines();
        switch (type)
        {
            case DoorType.DoorR:
                StartCoroutine(CloseDoorR());
                break;
            case DoorType.DoorL:
                StartCoroutine(CloseDoorL());
                break;
            case DoorType.Portcullis:
                StartCoroutine(ClosePortcullis());
                break;
        }
    }

    IEnumerator OpenDoorR()
    {
        while (transform.localEulerAngles.y > openOffset)
        {
            transform.Rotate(0, -1f, 0, Space.Self);

            yield return null;
        }
    }

    IEnumerator OpenDoorL()
    {
        while (transform.localEulerAngles.y < openOffset)
        {
            transform.Rotate(0, 1f, 0, Space.Self);

            yield return null;
        }
    }

    IEnumerator OpenPortcullis()
    {
        while (transform.position.y < openOffset)
        {
            Vector3 calcPosition = transform.position;
            calcPosition.y += 0.1f;
            transform.position = calcPosition;

            yield return null;
        }
    }

    IEnumerator CloseDoorR()
    {
        while (transform.localEulerAngles.y < closeOffset)
        {
            Vector3 calcRotation = transform.localEulerAngles;
            calcRotation.y -= 1f;
            transform.localEulerAngles = calcRotation;

            yield return null;
        }
    }

    IEnumerator CloseDoorL()
    {
        while (transform.localEulerAngles.y > closeOffset)
        {
            Vector3 calcRotation = transform.localEulerAngles;
            calcRotation.y -= 1f;
            transform.localEulerAngles = calcRotation;

            yield return null;
        }
    }

    IEnumerator ClosePortcullis()
    {
        while (transform.position.y > closeOffset)
        {
            Vector3 calcPosition = transform.position;
            calcPosition.y -= 0.0f;
            transform.position = calcPosition;

            yield return null;
        }
    }
}
