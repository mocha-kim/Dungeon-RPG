using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType : int
{
    Sword = 0,
    Shield = 1,
    Bag = 2,
    Totem = 3,
    Food,
    Default
}

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject
{
    public ItemType type;
    public bool isStackable;

    public Sprite icon;
    public GameObject modelPrefab;

    public Item data = new();

    public List<string> boneNames = new();

    [TextArea(15, 20)]
    public string description;

    private void OnValidate()
    {
        boneNames.Clear();

        if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null)
            return;

        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;

        foreach (Transform t in bones)
        {
            boneNames.Add(t.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new(this);
        return newItem;
    }
}
