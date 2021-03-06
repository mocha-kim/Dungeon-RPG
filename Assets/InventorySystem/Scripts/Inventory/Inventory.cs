using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RPG.InventorySystem.Items;
using UnityEngine;

namespace RPG.InventorySystem.Inventory
{

    [Serializable]
    public class Inventory
    {
        public InventorySlot[] slots = new InventorySlot[8];

        public void Clear()
        {
            foreach (InventorySlot slot in slots)
                slot.RemoveItem();
        }

        public bool IsContain(ItemObject itemObject)
        {
            //return Array.Find(slots, i => i.item.id == itemObject.data.id) != null;
            return IsContain(itemObject.data.id);
        }

        public bool IsContain(int id)
        {
            return slots.FirstOrDefault(i => i.item.id == id) != null;
        }
    }

}