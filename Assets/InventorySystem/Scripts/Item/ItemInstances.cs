using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances : MonoBehaviour
{
    public List<Transform> items = new();

    private void OnDestroy()
    {
        foreach (Transform item in items)
        {
            Destroy(item.gameObject);
        }
    }
}
