using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RPG.InventorySystem.Inventory
{

    public class StaticInventory : InventoryUI
    {
        public GameObject[] staticSlots = null;

        public override void CreateSlots()
        {
            slots = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0; i < inventoryObject.Slots.Length; ++i)
            {
                GameObject uiGo = staticSlots[i];

                AddEvent(uiGo, EventTriggerType.PointerEnter, delegate { OnEnterSlot(uiGo); });
                AddEvent(uiGo, EventTriggerType.PointerExit, delegate { OnExitSlot(uiGo); });
                AddEvent(uiGo, EventTriggerType.BeginDrag, delegate { OnStartDrag(uiGo); });
                AddEvent(uiGo, EventTriggerType.Drag, delegate { OnDrag(uiGo); });
                AddEvent(uiGo, EventTriggerType.EndDrag, delegate { OnEndDrag(uiGo); });

                inventoryObject.Slots[i].slotUI = uiGo;
                slots.Add(uiGo, inventoryObject.Slots[i]);
            }
        }
    }

}