using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Door Event", menuName = "Event System/Door Event")]
public class DoorEventObject : ScriptableObject
{
    [NonSerialized]
    public Action<int> OnOpenDoor;

    [NonSerialized]
    public Action<int> OnCloseDoor;

    public void OpenDoor(int id)
    {
        OnOpenDoor?.Invoke(id);
    }

    public void ClosenDoor(int id)
    {
        OnCloseDoor?.Invoke(id);
    }
}
