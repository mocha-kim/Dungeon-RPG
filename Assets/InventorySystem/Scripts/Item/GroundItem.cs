using System.Collections;
using System.Collections.Generic;
using RPG.Cameras;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public ItemObject itemObject;
    public GameObject particlePrefab;

    private void OnValidate()
    {
#if UNITY_EDITOR
        GetComponent<SpriteRenderer>().sprite = itemObject.icon;
#endif
    }

    private void Start()
    {
        GameObject particle = Instantiate(particlePrefab, transform.position, Quaternion.identity);
        particle.transform.parent = transform;
    }
}
