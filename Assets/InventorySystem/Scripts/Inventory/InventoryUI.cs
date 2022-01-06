using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace RPG.InventorySystem.Inventory
{

    public static class MouseData
    {
        public static InventoryUI interfaceMouseHovered;
        public static GameObject slotMouseHovered;
        public static GameObject tempItemDragging;
    }

    [RequireComponent(typeof(EventTrigger))]
    public abstract class InventoryUI : MonoBehaviour
    {
        public InventoryObject inventoryObject;
        private InventoryObject previousInventoryObject;

        public Dictionary<GameObject, InventorySlot> slots = new Dictionary<GameObject, InventorySlot>();

        private void Awake()
        {
            CreateSlots();

            for (int i = 0; i < inventoryObject.Slots.Length; ++i)
            {
                inventoryObject.Slots[i].parent = inventoryObject;
                inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;
            }

            AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
            AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });

        }

        protected virtual void Start()
        {
            for (int i = 0; i < inventoryObject.Slots.Length; ++i)
            {
                inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
            }
        }

        public abstract void CreateSlots();

        protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = go.GetComponent<EventTrigger>();
            if (!trigger)
            {
                Debug.LogWarning("No EventTrigger component found");
                return;
            }

            EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void OnPostUpdate(InventorySlot slot)
        {
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().sprite = slot.item.id < 0 ? null : slot.ItemObject.icon;
            slot.slotUI.transform.GetChild(0).GetComponent<Image>().color = slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);
            slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text = slot.item.id < 0 ? string.Empty : (slot.amount == 1 ? string.Empty : slot.amount.ToString());
        }

        public void OnEnterInterface(GameObject go)
        {
            MouseData.interfaceMouseHovered = go.GetComponent<InventoryUI>();
        }


        public void OnExitInterface(GameObject go)
        {
            MouseData.interfaceMouseHovered = null;
        }

        public void OnEnterSlot(GameObject go)
        {
            MouseData.slotMouseHovered = go;
        }

        public void OnExitSlot(GameObject go)
        {
            MouseData.slotMouseHovered = null;
        }

        public void OnStartDrag(GameObject go)
        {
            MouseData.tempItemDragging = CreateDragImage(go);
        }

        public void OnDrag(GameObject go)
        {
            if (MouseData.tempItemDragging == null)
                return;

            MouseData.tempItemDragging.GetComponent<RectTransform>().position = Input.mousePosition;
        }

        public void OnEndDrag(GameObject go)
        {
            Destroy(MouseData.tempItemDragging);

            if (MouseData.interfaceMouseHovered == null)
            {
                Debug.Log("call remove");
                slots[go].RemoveItem();
            }
            else if (MouseData.slotMouseHovered != null)
            {
                Debug.Log("call swap");
                InventorySlot mouseHoverSlotData = MouseData.interfaceMouseHovered.slots[MouseData.slotMouseHovered];
                inventoryObject.SwapItems(slots[go], mouseHoverSlotData);
            }
        }

        private GameObject CreateDragImage(GameObject go)
        {
            if (slots[go].item.id < 0)
                return null;

            GameObject dragImageGo = new GameObject();

            RectTransform rect = dragImageGo.AddComponent<RectTransform>();
            rect.sizeDelta = new Vector3(50, 50);
            dragImageGo.transform.SetParent(transform.parent);

            Image image = dragImageGo.AddComponent<Image>();
            image.sprite = slots[go].ItemObject.icon;
            image.raycastTarget = false;

            dragImageGo.name = "Drag Image";

            return dragImageGo;
        }

        public void OnClick(GameObject go, PointerEventData data)
        {
            InventorySlot slot = slots[go];
            if (slot == null)
                return;

            if (data.button == PointerEventData.InputButton.Left)
                OnLeftClick(slot);

            if (data.button == PointerEventData.InputButton.Right)
                OnRightClick(slot);
        }

        protected virtual void OnLeftClick(InventorySlot slot)
        { }

        protected virtual void OnRightClick(InventorySlot slot)
        { }
    }

}