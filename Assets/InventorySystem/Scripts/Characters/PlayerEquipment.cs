using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject equipment;
    private EquipmentCombiner combiner;
    private ItemInstances[] itemInstances = new ItemInstances[4];

    public ItemObject[] defaultItemObjects = new ItemObject[4];

    private bool isNewbie = true;

    private void Awake()
    {
        combiner = new EquipmentCombiner(gameObject);

        for (int i = 0; i < equipment.Slots.Length; ++i)
        {
            equipment.Slots[i].OnPreUpdate += OnRemoveItem;
            equipment.Slots[i].OnPostUpdate += OnEquipItem;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (InventorySlot slot in equipment.Slots)
        {
            if (isNewbie)
            {
                if (slot.item == null)
                    slot.AddItem(defaultItemObjects[(int)slot.allowedItems[0]].data, 1);
                return;
            }
            isNewbie = false;
            OnEquipItem(slot);
        }

    }

    private void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        //if (itemObject == null)
        //{
        //    EquipDefaultItemBy(slot.allowedItems[0]);
        //    return;
        //}

        int index = (int)slot.allowedItems[0];
        switch(slot.allowedItems[0])
        {
            // case skinned mesh:
            // itemInstances[index] = EqupSkinnedItem(itemObects);
            case ItemType.Sword:
            case ItemType.Shield:
            case ItemType.Bag:
            case ItemType.Totem:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }

        if (itemInstances[index] != null)
            itemInstances[index].name = slot.allowedItems[0].ToString();
    }

    private ItemInstances EquipSkinnedItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform itemTransform = combiner.AddLimb(itemObject.modelPrefab, itemObject.boneNames);
        ItemInstances instance = itemTransform.gameObject.AddComponent<ItemInstances>();
        if (instance != null)
            instance.items.Add(itemTransform);

        return instance;
    }

    private ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)
            return null;

        Transform[] itemTransforms = combiner.AddMesh(itemObject.modelPrefab);
        if (itemTransforms.Length > 0)
        {
            ItemInstances instance = new GameObject().AddComponent<ItemInstances>();
            foreach (Transform t in itemTransforms)
                instance.items.Add(t);
            instance.transform.parent = transform;

            return instance;
        }

        return null;
    }

    private void EquipDefaultItemBy(ItemType type)
    {
        int index = (int)type;
        ItemObject itemObject = defaultItemObjects[index];
        switch (type)
        {
            // case skinned mesh:
            // itemInstances[index] = EqupSkinnedItem(itemObects);
            case ItemType.Sword:
            case ItemType.Shield:
            case ItemType.Bag:
            case ItemType.Totem:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }

        if (slot.ItemObject.modelPrefab != null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }   
    }

    private void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        if (itemInstances[index] != null)
        {
            Destroy(itemInstances[index].gameObject);
            itemInstances[index] = null;
        }
    }
}
