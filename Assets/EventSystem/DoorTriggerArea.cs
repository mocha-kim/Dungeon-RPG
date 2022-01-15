using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTriggerArea : MonoBehaviour
{
    public DoorEventObject doorEventObject;
    public DoorController[] doorControllers;

    public bool autoClose = true;

    private void OnTriggerEnter(Collider other)
    {
        foreach (DoorController controller in doorControllers)
            doorEventObject.OpenDoor(controller.id);
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (DoorController controller in doorControllers)
            doorEventObject.ClosenDoor(controller.id);
    }
}
